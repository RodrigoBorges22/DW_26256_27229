using Microsoft.AspNetCore.Mvc.RazorPages;
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
            if (role != "Admin") query = query.Where(e => e.UtilizadorId == userId);
            Evento = await query.ToListAsync();
        }
    }
}