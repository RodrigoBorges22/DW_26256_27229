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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define a chave primária composta para a tabela de junção M-N
        modelBuilder.Entity<Inscricao>()
                .HasKey(i => new { i.EventoId, i.UtilizadorId });
                
        // (A injeção de dados foi movida para o SeedData.cs para não poluir as Migrations)
    }
}