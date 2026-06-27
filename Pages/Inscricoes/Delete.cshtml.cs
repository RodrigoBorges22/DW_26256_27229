using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.SignalR;
using DW_26256_27229.Hubs;
namespace DW_26256_27229.Pages_Inscricoes
{
    public class DeleteModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        private readonly IHubContext<NotificacaoHub> _hubContext;
        public DeleteModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) { _context = context; _hubContext = hubContext; }
        [BindProperty]
        public Inscricao Inscricao { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var inscricao = await _context.Inscricoes.Include(i => i.Utilizador).Include(i => i.Evento).FirstOrDefaultAsync(m => m.Id == id);
            if (inscricao != null) { Inscricao = inscricao; return Page(); }
            return NotFound();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao != null)
            {
                Inscricao = inscricao;
                _context.Inscricoes.Remove(Inscricao);
                await _context.SaveChangesAsync();
                var lotacaoReal = await _context.Inscricoes.CountAsync(i => i.EventoId == inscricao.EventoId);
                await _hubContext.Clients.All.SendAsync("AtualizarVagas", inscricao.EventoId, lotacaoReal);
            }
            return RedirectToPage("./Index");
        }
    }
}