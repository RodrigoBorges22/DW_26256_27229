using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DW_26256_27229.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DW_26256_27229.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Tipo { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Procurar na base de dados um utilizador com este Email e Password
            var utilizador = _context.Utilizadores
                .FirstOrDefault(u => u.Email == Email && u.Password == Password);

            // 2. Se não existir, a porta não abre!
            if (utilizador == null)
            {
                ModelState.AddModelError(string.Empty, "Email ou palavra-passe incorretos.");
                return Page();
            }

            // 3. O utilizador existe! Vamos criar o "Cartão de Acesso" (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, utilizador.Nome),
                new Claim(ClaimTypes.Email, utilizador.Email),
                new Claim(ClaimTypes.Role, utilizador.TipoUtilizador), // importante para as permissões
                new Claim("UtilizadorId", utilizador.Id.ToString())    // Vai dar muito jeito para as inscrições
            };

            var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identidade);

            // 4. Registar a entrada (criar o Cookie no browser)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // 5. Sucesso, Redirecionar para a página principal
            return RedirectToPage("/Index");
        }
    }
}