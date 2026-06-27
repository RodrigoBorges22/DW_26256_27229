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

namespace DW_26256_27229.Pages_Inscricoes
{
    [Authorize(Roles = "Aluno")]
    public class IndexModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Inscricao> Inscricao { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)!.Value;
            var userId = _context.Utilizadores.First(u => u.Email == email).Id;
            var query = _context.Inscricoes.Include(i => i.Evento).Include(i => i.Utilizador).AsQueryable();
            if (role == "Aluno") query = query.Where(i => i.UtilizadorId == userId);
            Inscricao = await query.ToListAsync();
        }
    }
}
