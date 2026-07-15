using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Controllers
{
    [Authorize(Roles = "Professor, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InscricoesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InscricoesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoes()
        {
            return await _context.Inscricoes.ToListAsync();
        }

        // GET: api/InscricoesApi/5/10  (Passa a receber o ID do Evento e do Utilizador)
        [HttpGet("{eventoId}/{utilizadorId}")]
        public async Task<ActionResult<Inscricao>> GetInscricao(int eventoId, int utilizadorId)
        {
            // Usa o FindAsync com as duas chaves compostas
            var inscricao = await _context.Inscricoes.FindAsync(eventoId, utilizadorId);

            if (inscricao == null)
            {
                return NotFound();
            }

            return inscricao;
        }

        // PUT: api/InscricoesApi/5/10
        [HttpPut("{eventoId}/{utilizadorId}")]
        public async Task<IActionResult> PutInscricao(int eventoId, int utilizadorId, Inscricao inscricao)
        {
            // Valida se as chaves batem certo com o objeto enviado
            if (eventoId != inscricao.EventoId || utilizadorId != inscricao.UtilizadorId)
            {
                return BadRequest();
            }

            _context.Entry(inscricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoExists(eventoId, utilizadorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/InscricoesApi
        [HttpPost]
        public async Task<ActionResult<Inscricao>> PostInscricao(Inscricao inscricao)
        {
            _context.Inscricoes.Add(inscricao);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InscricaoExists(inscricao.EventoId, inscricao.UtilizadorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            // Ao devolver o caminho de criação, envia ambas as chaves
            return CreatedAtAction("GetInscricao", new { eventoId = inscricao.EventoId, utilizadorId = inscricao.UtilizadorId }, inscricao);
        }

        // DELETE: api/InscricoesApi/5/10
        [HttpDelete("{eventoId}/{utilizadorId}")]
        public async Task<IActionResult> DeleteInscricao(int eventoId, int utilizadorId)
        {
            var inscricao = await _context.Inscricoes.FindAsync(eventoId, utilizadorId);
            if (inscricao == null)
            {
                return NotFound();
            }

            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar atualizado para as duas chaves
        private bool InscricaoExists(int eventoId, int utilizadorId)
        {
            return _context.Inscricoes.Any(e => e.EventoId == eventoId && e.UtilizadorId == utilizadorId);
        }
    }
}