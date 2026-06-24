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
}