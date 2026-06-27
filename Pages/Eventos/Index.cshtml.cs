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
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        private readonly IHubContext<NotificacaoHub> _hubContext;
        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) { _context = context; _hubContext = hubContext; }
        public IList<Evento> Evento { get; set; } = default!;
        public async Task OnGetAsync()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UtilizadorId")!.Value);
            var query = _context.Eventos.Include(e => e.Categoria).Include(e => e.Inscricoes).AsQueryable();
            if (role == "Professor") query = query.Where(e => e.UtilizadorId == userId);
            Evento = await query.ToListAsync();
        }
        public async Task<IActionResult> OnPostInscreverAsync(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var alunoId = _context.Utilizadores.First(u => u.Email == email).Id;
            var evento = await _context.Eventos.Include(e => e.Inscricoes).FirstAsync(e => e.Id == id);
            if (evento.Inscricoes.Any(i => i.UtilizadorId == alunoId))
            {
                TempData["MensagemErro"] = "Não é possível inscrever, já se encontra inscrito neste evento.";
                return RedirectToPage();
            }
            if (evento.Inscricoes.Count >= evento.VagasMaximas)
            {
                TempData["MensagemErro"] = "Este evento já está lotado!";
                return RedirectToPage();
            }
            _context.Inscricoes.Add(new Inscricao { EventoId = id, UtilizadorId = alunoId, DataInscricao = DateTime.Now });
            await _context.SaveChangesAsync();
            var lotacaoReal = await _context.Inscricoes.CountAsync(i => i.EventoId == id);
            await _hubContext.Clients.All.SendAsync("AtualizarVagas", id, lotacaoReal);
            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToPage();
        }
    }
}