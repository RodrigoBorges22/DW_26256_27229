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
    public class IndexModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Categoria> Categoria { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Categoria = await _context.Categorias.ToListAsync();
        }
    }
}
