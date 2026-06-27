using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using DW_26256_27229.Hubs;
namespace DW_26256_27229.Pages_Eventos
{
    public class IndexModel : PageModel
    {
        // Contexto de BD e o HubContext para enviar mensagens via SignalR
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        private readonly IHubContext<NotificacaoHub> _hubContext;
        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) { _context = context; _hubContext = hubContext; }
        public IList<Evento> Evento { get; set; } = default!;

        // Carrega a lista de eventos, aplicando filtros se o utilizador for Professor
        public async Task OnGetAsync()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UtilizadorId")!.Value);
            // Query base com carregamento dos dados relacionados (Categoria e Inscrições)
            var query = _context.Eventos.Include(e => e.Categoria).Include(e => e.Inscricoes).AsQueryable();
            // Se for Professor, filtra a lista para mostrar apenas os eventos que ele criou
            if (role == "Professor") query = query.Where(e => e.UtilizadorId == userId);
            Evento = await query.ToListAsync();
        }

        // Lógica de inscrição de um aluno num evento
        public async Task<IActionResult> OnPostInscreverAsync(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var alunoId = _context.Utilizadores.First(u => u.Email == email).Id;
            var evento = await _context.Eventos.Include(e => e.Inscricoes).FirstAsync(e => e.Id == id);

            // Regra: Verifica se o aluno já está inscrito
            if (evento.Inscricoes.Any(i => i.UtilizadorId == alunoId))
            {
                TempData["MensagemErro"] = "Não é possível inscrever, já se encontra inscrito neste evento.";
                return RedirectToPage();
            }

            // Regra: Verifica se o evento atingiu a lotação máxima
            if (evento.Inscricoes.Count >= evento.VagasMaximas)
            {
                TempData["MensagemErro"] = "Este evento já está lotado!";
                return RedirectToPage();
            }

            // Adiciona a inscrição e guarda na BD
            _context.Inscricoes.Add(new Inscricao { EventoId = id, UtilizadorId = alunoId, DataInscricao = DateTime.Now });
            await _context.SaveChangesAsync();

            // Calcula a nova contagem de inscrições e notifica todos os clientes via SignalR
            var lotacaoReal = await _context.Inscricoes.CountAsync(i => i.EventoId == id);
            await _hubContext.Clients.All.SendAsync("AtualizarVagas", id, lotacaoReal);
            
            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToPage();
        }
    }
}