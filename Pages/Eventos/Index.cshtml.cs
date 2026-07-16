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

        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) 
        { 
            _context = context; 
            _hubContext = hubContext; 
        }

        public IList<Evento> Evento { get; set; } = default!;

        // --- VARIÁVEIS DE PAGINAÇÃO ---
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        // Carrega a lista de eventos, aplicando filtros de Professor e Paginação
        public async Task OnGetAsync(int? pageIndex)
        {
            // 1. Configuração base da Paginação
            PageIndex = pageIndex ?? 1;
            int pageSize = 4; // Número de eventos por página (ajusta conforme preferires)

            // 2. Extração de dados do Utilizador
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Parse seguro para evitar erro caso o Id não esteja disponível
            int userId = 0;
            var userIdClaim = User.FindFirst("UtilizadorId")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                userId = int.Parse(userIdClaim);
            }

            // 3. Query base
            var query = _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Inscricoes)
                .OrderBy(e => e.DataHora)
                .AsQueryable();

            // 4. Aplica o filtro se for Professor
            if (role == "Professor") 
            {
                query = query.Where(e => e.UtilizadorId == userId);
            }

            // 5. Conta o total de eventos APÓS aplicar o filtro (para calcular as páginas)
            var totalEventos = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalEventos / (double)pageSize);

            // 6. Aplica a Paginação e vai à Base de Dados
            Evento = await query
                .Skip((PageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Lógica de inscrição de um aluno num evento
        public async Task<IActionResult> OnPostInscreverAsync(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var alunoId = _context.Utilizadores.First(u => u.Email == email).Id;
            var evento = await _context.Eventos.Include(e => e.Inscricoes).FirstAsync(e => e.Id == id);

            //Verifica se o aluno já está inscrito
            if (evento.Inscricoes.Any(i => i.UtilizadorId == alunoId))
            {
                TempData["MensagemErro"] = "Não é possível inscrever, já se encontra inscrito neste evento.";
                return RedirectToPage();
            }

            //Verifica se o evento atingiu a lotação máxima
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