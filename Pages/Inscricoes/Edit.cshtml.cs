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

namespace DW_26256_27229.Pages_Inscricoes
{
    public class EditModel : PageModel
    {
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public EditModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Permite que os dados do formulário da página sejam carregados automaticamente para este objeto
        [BindProperty]
        public Inscricao Inscricao { get; set; } = default!;

        // Método chamado quando SE entra na página (carrega os dados atuais da inscrição para o formulário)
        public async Task<IActionResult> OnGetAsync(int? eventoId, int? utilizadorId)
        {
            if (eventoId == null || utilizadorId == null)
            {
                return NotFound(); // Erro se faltar algum dos IDs
            }

            // Procura usando a chave composta
            var inscricao =  await _context.Inscricoes.FirstOrDefaultAsync(m => m.EventoId == eventoId && m.UtilizadorId == utilizadorId);
            
            if (inscricao == null)
            {
                return NotFound(); // Erro se a inscrição não existir na base de dados
            }
            
            Inscricao = inscricao;
            
            // Prepara as listas para os dropdowns (selects) de Eventos e Utilizadores na interface
            ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "Id");
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadores, "Id", "Id");
            return Page();
        }

        // Método chamado quando se carregar no botão de "Guardar" (processa o formulário)
        public async Task<IActionResult> OnPostAsync()
        {
            // Valida se os dados inseridos respeitam as regras do modelo (ex: campos obrigatórios)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Marca o objeto como modificado no contexto da base de dados
            _context.Attach(Inscricao).State = EntityState.Modified;

            try
            {
                // Tenta guardar as alterações na base de dados
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se a inscrição ainda existe usando as duas chaves
                if (!InscricaoExists(Inscricao.EventoId, Inscricao.UtilizadorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redireciona de volta para a lista de inscrições após sucesso
            return RedirectToPage("./Index");
        }

        // Método auxiliar atualizado para verificar a existência de uma inscrição com chave composta
        private bool InscricaoExists(int eventoId, int utilizadorId)
        {
            return _context.Inscricoes.Any(e => e.EventoId == eventoId && e.UtilizadorId == utilizadorId);
        }
    }
}