using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages
{
    public class RegistoModel : PageModel
    {
        // 1. ponte para a base de dados
        private readonly ApplicationDbContext _context;

        public RegistoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Tipo { get; set; }

        [BindProperty]
        public string Nome { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Tipo))
            {
                Tipo = "Aluno";
            }
        }

        // 2. O botão "Criar Conta" aciona este método
        public async Task<IActionResult> OnPostAsync()
        {
            // Verifica se o email já existe na base de dados
            var emailJaExiste = _context.Utilizadores.Any(u => u.Email == Email);
            if (emailJaExiste)
            {
                // Devolve um erro visual debaixo da caixa do email
                ModelState.AddModelError("Email", "Este email já está registado na plataforma.");
                return Page();
            }

            // Constrói o novo utilizador com os dados do formulário
            var novoUtilizador = new Utilizador
            {
                Nome = Nome,
                Email = Email,
                Password = Password, // Num projeto empresarial real, aqui encriptáva-se a password, decidimos não encriptar
                TipoUtilizador = Tipo
            };

            // Guarda na base de dados
            _context.Utilizadores.Add(novoUtilizador);
            await _context.SaveChangesAsync();

            // Depois de criar conta com sucesso, atira o utilizador para a página de Login
            return RedirectToPage("/Login", new { tipo = Tipo });
        }
    }
}