using DW_26256_27229.Models;
using Microsoft.EntityFrameworkCore;

namespace DW_26256_27229.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Verifica se a base de dados já tem dados. Se sim, aborta a injeção.
                if (context.Categorias.Any() || context.Utilizadores.Any() || context.Eventos.Any())
                {
                    return; 
                }

                // 1. Injetar Categorias
                var categorias = new List<Categoria>
                {
                    new Categoria { Nome = "Workshop Técnico" },
                    new Categoria { Nome = "Palestra" },
                    new Categoria { Nome = "Torneio de Gaming" }
                };
                context.Categorias.AddRange(categorias);
                context.SaveChanges();

                // 2. Injetar Utilizadores de Teste
                var utilizadores = new List<Utilizador>
                {
                    new Utilizador { Nome = "Administrador Principal", Email = "admin@ipt.pt", TipoUtilizador = "Admin", Password = "123" },
                    new Utilizador { Nome = "Aluno Teste", Email = "aluno@ipt.pt", TipoUtilizador = "Aluno", Password = "123" },
                    new Utilizador { Nome = "Professor Teste", Email = "professor@ipt.pt", TipoUtilizador = "Professor", Password = "123" }
                };
                context.Utilizadores.AddRange(utilizadores);
                context.SaveChanges();

                // 3. Injetar Eventos
                // buscar os IDs reais que o SQLite acabou de gerar para garantir as ligações corretas
                var idCategoriaWorkshop = context.Categorias.FirstOrDefault(c => c.Nome == "Workshop Técnico")!.Id;
                var idCategoriaGaming = context.Categorias.FirstOrDefault(c => c.Nome == "Torneio de Gaming")!.Id;
                var idAdmin = context.Utilizadores.FirstOrDefault(u => u.Email == "admin@ipt.pt")!.Id;

                var eventos = new List<Evento>
                {
                    new Evento { 
                        Titulo = "Introdução a Redes Cisco e Routing", 
                        DataHora = new DateTime(2026, 10, 15, 14, 0, 0), 
                        VagasMaximas = 20, 
                        CategoriaId = idCategoriaWorkshop, 
                        UtilizadorId = idAdmin
                    },
                    new Evento { 
                        Titulo = "Torneio Local Co-op (Rayman Legends)", 
                        DataHora = new DateTime(2026, 11, 10, 18, 30, 0), 
                        VagasMaximas = 16, 
                        CategoriaId = idCategoriaGaming,  
                        UtilizadorId = idAdmin
                    }
                };
                context.Eventos.AddRange(eventos);
                context.SaveChanges();
            }
        }
    }
}