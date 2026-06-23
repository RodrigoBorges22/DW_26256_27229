using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;

namespace DW_26256_27229.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Utilizador> Utilizadores { get; set; }
    public DbSet<Inscricao> Inscricoes { get; set; }
}