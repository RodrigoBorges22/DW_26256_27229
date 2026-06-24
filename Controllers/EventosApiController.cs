using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using DW_26256_27229.Models;

namespace DW_26256_27229.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosApiController : ControllerBase
{
    // A nossa "chave" para entrar na base de dados
    private readonly ApplicationDbContext _context;

    public EventosApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/EventosApi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
    {
        // Vai à base de dados, pega em todos os eventos, 
        // inclui a informação da Categoria associada, e devolve como lista JSON
        return await _context.Eventos.Include(e => e.Categoria).ToListAsync();
    }

    // POST: api/EventosApi (Criar um evento novo)
    [HttpPost]
    public async Task<ActionResult<Evento>> PostEvento(Evento evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEventos), new { id = evento.Id }, evento);
    }

    // DELETE: api/EventosApi/3 (Apagar um evento pelo ID)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvento(int id)
    {
        // 1. Vai à base de dados procurar o evento com este ID
        var evento = await _context.Eventos.FindAsync(id);
        
        // 2. Se não encontrar nada, avisa que não existe (Erro 404)
        if (evento == null)
        {
            return NotFound();
        }

        // 3. Se encontrar, dá ordem de abate e guarda as alterações
        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        // 4. Devolve um código 204 (Sucesso, mas sem conteúdo para mostrar)
        return NoContent();
    }

    // PUT: api/EventosApi/2 (Editar um evento existente)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvento(int id, Evento evento)
    {
        // 1. Segurança: Verifica se o ID que vem no link é igual ao ID que vem dentro do JSON
        if (id != evento.Id)
        {
            return BadRequest(); // Erro 400 se não baterem certo
        }

        // 2. Avisar o motor do Entity Framework que este objeto foi modificado
        _context.Entry(evento).State = EntityState.Modified;

        // 3. Guardar as alterações
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Se o evento já não existir na base de dados quando tentar guardar
            if (!_context.Eventos.Any(e => e.Id == id))
            {
                return NotFound(); // Erro 404
            }
            else
            {
                throw;
            }
        }

        // 4. Sucesso (Código 204)
        return NoContent();
    }
}