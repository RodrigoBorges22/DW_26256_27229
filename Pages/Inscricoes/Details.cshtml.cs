using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages_Inscricoes
{
    public class DetailsModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public DetailsModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Inscricao Inscricao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes.FirstOrDefaultAsync(m => m.Id == id);

            if (inscricao is not null)
            {
                Inscricao = inscricao;

                return Page();
            }

            return NotFound();
        }
    }
}
