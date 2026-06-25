using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
namespace DW_26256_27229.Pages_Categorias
{
    public class DeleteModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        public DeleteModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Categoria Categoria { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
            if (categoria != null) { Categoria = categoria; return Page(); }
            return NotFound();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var temEventos = await _context.Eventos.AnyAsync(e => e.CategoriaId == id);
            if (temEventos)
            {
                ModelState.AddModelError(string.Empty, "Não é possível eliminar a categoria porque tem eventos associados.");
                Categoria = await _context.Categorias.FindAsync(id);
                return Page();
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                Categoria = categoria;
                _context.Categorias.Remove(Categoria);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}