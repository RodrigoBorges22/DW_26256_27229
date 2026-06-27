using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Pages_Inscricoes
{
    public class CreateModel : PageModel
    {
        // Contexto para acesso à base de dados
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public CreateModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Método chamado no carregamento inicial da página (GET)
        public IActionResult OnGet()
        {
            // Prepara as listas de seleção para escolher o Evento e o Utilizador
            ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "Id");
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadores, "Id", "Id");
            return Page();
        }

        // Mapeia os campos do formulário para o objeto 'Inscricao'
        [BindProperty]
        public Inscricao Inscricao { get; set; } = default!;

        // Método chamado ao submeter o formulário (POST)
        public async Task<IActionResult> OnPostAsync()
        {
            // Verifica se os dados submetidos são válidos de acordo com o modelo
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Adiciona a nova inscrição ao contexto e guarda as alterações na base de dados
            _context.Inscricoes.Add(Inscricao);
            await _context.SaveChangesAsync();

            // Redireciona o utilizador de volta para a listagem principal
            return RedirectToPage("./Index");
        }
    }
}
