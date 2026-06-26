using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using System.Security.Claims;
namespace DW_26256_27229.Pages_Eventos
{
    public class IndexModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context) { _context = context; }
        public IList<Evento> Evento { get; set; } = default!;
        public async Task OnGetAsync()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst("UtilizadorId")!.Value);
            var query = _context.Eventos.Include(e => e.Categoria).AsQueryable();
            if (role == "Professor") query = query.Where(e => e.UtilizadorId == userId);
            Evento = await query.ToListAsync();
        }
        public async Task<IActionResult> OnPostInscreverAsync(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var alunoId = _context.Utilizadores.First(u => u.Email == email).Id;
            var existe = await _context.Inscricoes.AnyAsync(i => i.EventoId == id && i.UtilizadorId == alunoId);
            if (!existe)
            {
                _context.Inscricoes.Add(new Inscricao { EventoId = id, UtilizadorId = alunoId, DataInscricao = DateTime.Now });
                await _context.SaveChangesAsync();

                TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Não é possível inscrever, já te encontras inscrito neste evento.";
            }
            return RedirectToPage();
        }
    }
}