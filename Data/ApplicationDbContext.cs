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

    // NOVA PARTE: A injeção de dados automáticos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Injetar Categorias
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Workshop Técnico" },
            new Categoria { Id = 2, Nome = "Palestra" },
            new Categoria { Id = 3, Nome = "Torneio de Gaming" }
        );

        // 2. Injetar Utilizadores de Teste
        modelBuilder.Entity<Utilizador>().HasData(
            new Utilizador { Id = 1, Nome = "Administrador Principal", Email = "admin@ipt.pt", TipoUtilizador = "Admin",Password = "123" },
            new Utilizador { Id = 2, Nome = "Aluno Teste", Email = "aluno@ipt.pt", TipoUtilizador = "Aluno", Password = "123" }
        );

        // 3. Injetar Eventos
        modelBuilder.Entity<Evento>().HasData(
            new Evento { 
                Id = 1, 
                Titulo = "Introdução a Redes Cisco e Routing", 
                DataHora = new DateTime(2026, 10, 15, 14, 0, 0), 
                VagasMaximas = 20, 
                CategoriaId = 1, 
                UtilizadorId=1
            },
            new Evento { 
                Id = 2, 
                Titulo = "Torneio Local Co-op (Rayman Legends)", 
                DataHora = new DateTime(2026, 11, 10, 18, 30, 0), 
                VagasMaximas = 16, 
                CategoriaId = 3,  
                UtilizadorId=1
            }
        );
    }
}