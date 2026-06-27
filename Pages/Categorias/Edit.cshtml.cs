using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;

namespace DW_26256_27229.Pages_Categorias
{
    // Acesso restrito a professores e administradores
    [Authorize(Roles = "Professor, Admin")]
    public class EditModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public EditModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;

        // Vai busca a categoria na BD para edição
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();
            Categoria = categoria;
            return Page();
        }

        // Processa as alterações e guarda na BD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Attach(Categoria).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se a categoria ainda existe em caso de erro de concorrência
                if (!CategoriaExists(Categoria.Id)) return NotFound();
                else throw;
            }
            return RedirectToPage("./Index");
        }

        // Método auxiliar para verificar existência
        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}