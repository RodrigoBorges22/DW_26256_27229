using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DW_26256_27229.Pages
{
    public class RegistoModel : PageModel
    {
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

        public IActionResult OnPost()
        {
            // Mais tarde, meter aqui a lógica de ligar à API/Base de Dados para gravar o utilizador
            return Page();
        }
    }
}