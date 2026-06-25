using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Controllers
{
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
            return await _context.Inscricoes
                .Include(i => i.Evento) // Traz os dados do evento associado
                .ToListAsync();
        }

        // GET: api/InscricoesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscricao>> GetInscricao(int id)
        {
            var inscricao = await _context.Inscricoes.FindAsync(id);

            if (inscricao == null)
            {
                return NotFound();
            }

            return inscricao;
        }

        // POST: api/InscricoesApi
        [HttpPost]
        public async Task<ActionResult<Inscricao>> PostInscricao(Inscricao inscricao)
        {
            // 1. Ir buscar o evento à base de dados para saber o limite de vagas
            var evento = await _context.Eventos.FindAsync(inscricao.EventoId);
            if (evento == null)
            {
                return NotFound("Erro: O evento que tentou aceder não existe.");
            }

            // 2. Contar quantas inscrições já existem para este evento específico
            var totalInscritos = await _context.Inscricoes
                .CountAsync(i => i.EventoId == inscricao.EventoId);

            // 3. Verificar se o evento já esgotou
            if (totalInscritos >= evento.VagasMaximas)
            {
                return BadRequest($"Inscrição recusada: O evento '{evento.Titulo}' já está lotado!");
            }

            // 4. Verificar se este utilizador já está inscrito neste evento
            var jaInscrito = await _context.Inscricoes
                .AnyAsync(i => i.EventoId == inscricao.EventoId && i.UtilizadorId == inscricao.UtilizadorId);

            if (jaInscrito)
            {
                return BadRequest("Inscrição recusada: Este utilizador já se encontra inscrito neste evento.");
            }

            // Se passou por todas as barreiras de segurança, guardar
            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInscricao), new { id = inscricao.Id }, inscricao);
        }

        // DELETE: api/InscricoesApi/5 (Desistir da inscrição)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscricao(int id)
        {
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound();
            }

            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}