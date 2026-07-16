using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DW_26256_27229.Data;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;

namespace DW_26256_27229.Pages_Categorias
{
    // Restringe acesso apenas a administradores ou professores
    [Authorize(Roles = "Professor, Admin")]
    public class CreateModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        // Injeção de dependência do contexto de dados
        public CreateModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Prepara a página para exibição
        public IActionResult OnGet()
        {
            return Page();
        }

        // Vincula os dados do formulário ao objeto Categoria
        [BindProperty]
        public Categoria Categoria { get; set; } = default!;

        // Processa o envio do formulário
        public async Task<IActionResult> OnPostAsync()
        {
            // Valida se os dados inseridos estão corretos
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Adiciona nova categoria e guarda na BD
            _context.Categorias.Add(Categoria);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Registo criado com sucesso!";

            // Volta para a lista de categorias
            return RedirectToPage("./Index");
        }
    }
}