using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;

namespace DW_26256_27229.Pages_Eventos
{
    // Acesso restrito a professores e administradores
    [Authorize(Roles = "Professor, Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        public DeleteModel(DW_26256_27229.Data.ApplicationDbContext context) { _context = context; }

        [BindProperty]
        public Evento Evento { get; set; } = default!;

        // Carrega os dados do evento para confirmar eliminação
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento is not null)
            {
                Evento = evento;
                return Page();
            }
            return NotFound();
        }

        // Processa a eliminação do evento na base de dados
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                Evento = evento;
                _context.Eventos.Remove(Evento);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}