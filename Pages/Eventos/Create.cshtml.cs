using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;

namespace DW_26256_27229.Pages_Eventos
{
    [Authorize(Roles = "Professor, Admin")]
    public class CreateModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        public CreateModel(DW_26256_27229.Data.ApplicationDbContext context) { _context = context; }
        public IActionResult OnGet()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return Page();
        }
        [BindProperty]
        public Evento Evento { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)!.Value;
            Evento.UtilizadorId = _context.Utilizadores.First(u => u.Email == email).Id;
            ModelState.Remove("Evento.Utilizador");
            ModelState.Remove("Evento.Categoria");
            if (!ModelState.IsValid) return Page();
            _context.Eventos.Add(Evento);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}