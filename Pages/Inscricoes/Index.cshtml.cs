using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace DW_26256_27229.Pages_Inscricoes
{
    // Define que apenas utilizadores com a role "Aluno" podem aceder a esta página
    [Authorize(Roles = "Aluno")]
    public class IndexModel : PageModel
    {
        // Variável de contexto para aceder à base de dados
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        // Construtor para injetar o contexto da base de dados
        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context) { _context = context; }
        // Lista que vai guardar as inscrições que serão mostradas na página
        public IList<Inscricao> Inscricao { get; set; } = default!;
        // Método que é executado quando o utilizador acede à página (GET)
        public async Task OnGetAsync()
        {
            // Obtém o cargo (role) e o email do utilizador logado através dos claims de autenticação
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            // Procura na base de dados o ID do utilizador correspondente ao email atual
            var userId = _context.Utilizadores.First(u => u.Email == email).Id;
            // Inicia uma query para buscar inscrições, incluindo os dados relacionados do Evento e Utilizador
            var query = _context.Inscricoes.Include(i => i.Evento).Include(i => i.Utilizador).AsQueryable();

            // Se for um aluno, filtra a lista para mostrar apenas as inscrições desse aluno
            if (role == "Aluno") query = query.Where(i => i.UtilizadorId == userId);
            Inscricao = await query.ToListAsync();
        }
    }
}