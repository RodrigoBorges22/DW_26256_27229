using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages_Categorias
{
    public class DetailsModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public DetailsModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Categoria Categoria { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);

            if (categoria is not null)
            {
                Categoria = categoria;

                return Page();
            }

            return NotFound();
        }
    }
}
