using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using DW_26256_27229.Hubs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DW_26256_27229.Pages_Eventos
{
    public class IndexModel : PageModel
    {
        // Contexto de BD e o HubContext para enviar mensagens via SignalR
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        private readonly IHubContext<NotificacaoHub> _hubContext;

        public IndexModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) 
        { 
            _context = context; 
            _hubContext = hubContext; 
        }

        public IList<Evento> Evento { get; set; } = default!;

        //VARIÁVEIS DE PAGINAÇÃO
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        //VARIÁVEIS DE FILTRO E PESQUISA
        public string SearchString { get; set; } = string.Empty;
        public int? SelectedCategoriaId { get; set; }
        public SelectList CategoriasDropdown { get; set; } = default!;

        // O OnGet agora recebe os parâmetros do formulário de pesquisa
        public async Task OnGetAsync(string? searchString, int? categoriaId, int? pageIndex)
        {
            // 1. Configuração base da Paginação e Filtros
            PageIndex = pageIndex ?? 1;
            int pageSize = 4;

            // Mantém os valores guardados para reatribuir aos inputs no HTML
            SearchString = searchString ?? string.Empty;
            SelectedCategoriaId = categoriaId;

            // Carrega as categorias para preencher a dropdown de pesquisa do topo
            var categorias = await _context.Categorias.OrderBy(c => c.Nome).ToListAsync();
            CategoriasDropdown = new SelectList(categorias, "Id", "Nome", categoriaId);

            // 2. Extração de dados do Utilizador
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            int userId = 0;
            var userIdClaim = User.FindFirst("UtilizadorId")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                userId = int.Parse(userIdClaim);
            }

            // 3. Query base
            var query = _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Inscricoes)
                .OrderBy(e => e.DataHora)
                .AsQueryable();

            // 4. Aplica o filtro se for Professor
            if (role == "Professor") 
            {
                query = query.Where(e => e.UtilizadorId == userId);
            }

            // 5. Aplica a pesquisa por TÍTULO (se o utilizador escreveu algo)
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(e => e.Titulo.ToLower().Contains(SearchString.ToLower()));
            }

            // 6. Aplica o filtro por CATEGORIA (se o utilizador escolheu uma)
            if (SelectedCategoriaId.HasValue)
            {
                query = query.Where(e => e.CategoriaId == SelectedCategoriaId.Value);
            }

            // 7. Conta o total de eventos APÓS os filtros todos estarem aplicados
            var totalEventos = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalEventos / (double)pageSize);

            //se os filtros reduzirem as páginas e ficarmos perdidos fora do range
            if (PageIndex > TotalPages && TotalPages > 0)
            {
                PageIndex = TotalPages;
            }

            // 8. Executa a paginação final
            Evento = await query
                .Skip((PageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Lógica de inscrição de um aluno num evento
        public async Task<IActionResult> OnPostInscreverAsync(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var alunoId = _context.Utilizadores.First(u => u.Email == email).Id;
            var evento = await _context.Eventos.Include(e => e.Inscricoes).FirstAsync(e => e.Id == id);

            if (evento.Inscricoes.Any(i => i.UtilizadorId == alunoId))
            {
                TempData["MensagemErro"] = "Não é possível inscrever, já se encontra inscrito neste evento.";
                return RedirectToPage();
            }

            if (evento.Inscricoes.Count >= evento.VagasMaximas)
            {
                TempData["MensagemErro"] = "Este evento já está lotado!";
                return RedirectToPage();
            }

            _context.Inscricoes.Add(new Inscricao { EventoId = id, UtilizadorId = alunoId, DataInscricao = DateTime.Now });
            await _context.SaveChangesAsync();

            var lotacaoReal = await _context.Inscricoes.CountAsync(i => i.EventoId == id);
            await _hubContext.Clients.All.SendAsync("AtualizarVagas", id, lotacaoReal);
            
            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToPage();
        }
    }
}