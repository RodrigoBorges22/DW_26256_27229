using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages_Eventos
{
    public class DetailsModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public DetailsModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Evento Evento { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Utilizador) // Puxa os dados do Professor que criou o evento
                .Include(e => e.Inscricoes) 
                .ThenInclude(i => i.Utilizador) // Puxa os dados dos Alunos inscritos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento is not null)
            {
                Evento = evento;

                return Page();
            }

            return NotFound();
        }
    }
}