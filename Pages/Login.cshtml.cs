using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DW_26256_27229.Pages
{
    public class LoginModel : PageModel
    {
        // Esta variável apanha o que vier no URL (ex: ?tipo=Professor)
        [BindProperty(SupportsGet = true)]
        public string Tipo { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
            // Se o utilizador tentar aceder ao login sem clicar nos botões, assumimos "Aluno" por defeito
            if (string.IsNullOrEmpty(Tipo))
            {
                Tipo = "Aluno";
            }
        }

        public IActionResult OnPost()
        {
            // depois é pra meter aqui a logica de ir a bd ver se a passwd está certa 
            // por agora, apenas recarrega a página
            return Page();
        }
    }
}