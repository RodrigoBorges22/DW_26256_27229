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
    // Restringe o acesso a esta página apenas a utilizadores com o perfil "Aluno"
    [Authorize(Roles = "Aluno")]
    public class DetailsModel : PageModel
    {
        // Contexto da base de dados para realizar consultas
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;

        public DetailsModel(DW_26256_27229.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // Objeto que conterá os dados da inscrição a visualizar
        public Inscricao Inscricao { get; set; } = default!;

        // Método acionado via HTTP GET para carregar os detalhes da inscrição pelas suas duas chaves
        public async Task<IActionResult> OnGetAsync(int? eventoId, int? utilizadorId)
        {
            // Verifica se ALGUM dos IDs está em falta; se sim, retorna erro 404
            if (eventoId == null || utilizadorId == null) 
            {
                return NotFound();
            }

            // Busca a inscrição na base de dados, combinando a pesquisa do EventoId com o UtilizadorId
            var inscricao = await _context.Inscricoes
                .Include(i => i.Evento)
                .Include(i => i.Utilizador)
                .FirstOrDefaultAsync(m => m.EventoId == eventoId && m.UtilizadorId == utilizadorId);

            // Se a inscrição não for encontrada, retorna erro 404
            if (inscricao == null) 
            {
                return NotFound();
            }
            
            // Atribui a inscrição encontrada à propriedade da página
            Inscricao = inscricao;
            
            // Retorna a página com os dados carregados
            return Page();
        }
    }
}