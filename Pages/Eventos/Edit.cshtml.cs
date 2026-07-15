using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.Authorization;

namespace DW_26256_27229.Pages_Eventos
{
    // Restringe o acesso a esta página apenas a utilizadores com perfil de "Professor" ou "Admin"
    [Authorize(Roles = "Professor, Admin")]
    public class EditModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public EditModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Associa os dados do formulário da página a este objeto Evento
        [BindProperty]
        public Evento Evento { get; set; } = default!;

        // Método para carregar os dados do evento que se pretende editar
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento =  await _context.Eventos.FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }
            Evento = evento;

            // Preenche o dropdown de categorias para o professor/admin escolher a categoria do evento
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return Page();
        }

        // Método chamado após submeter o formulário de edição
        public async Task<IActionResult> OnPostAsync()
        {
            // Verifica se o formulário cumpre os requisitos do modelo (validação)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Informa o contexto que o objeto Evento foi modificado
            _context.Attach(Evento).State = EntityState.Modified;

            try
            {
                // Tenta guardar as alterações na base de dados
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Tratamento de erro de concorrência: verifica se o evento ainda existe
                if (!EventoExists(Evento.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redireciona para a lista de eventos após sucesso
            return RedirectToPage("./Index");
        }

        // Função auxiliar para verificar se o evento existe
        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
