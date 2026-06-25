using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages_Eventos
{
    public class CreateModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public CreateModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Evento Evento { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Eventos.Add(Evento);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
