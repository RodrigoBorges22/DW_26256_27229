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

namespace DW_26256_27229.Pages_Categorias
{
    // Acesso restrito a professores e administradores
    [Authorize(Roles = "Professor, Admin")]
    public class IndexModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista para armazenar as categorias lidas da base de dados
        public IList<Categoria> Categoria { get; set; } = default!;

        // Vai buscar todas as categorias para exibir na listagem
        public async Task OnGetAsync()
        {
            Categoria = await _context.Categorias.ToListAsync();
        }
    }
}