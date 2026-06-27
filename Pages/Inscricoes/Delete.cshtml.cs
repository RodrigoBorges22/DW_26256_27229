using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Models;
using Microsoft.AspNetCore.SignalR;
using DW_26256_27229.Hubs;
namespace DW_26256_27229.Pages_Inscricoes
{
    public class DeleteModel : PageModel
    {
        // Contexto da base de dados e o Hub para comunicações em tempo real
        private readonly DW_26256_27229.Data.ApplicationDbContext _context;
        private readonly IHubContext<NotificacaoHub> _hubContext;

        // Construtor com injeção de dependência do contexto da BD e do Hub do SignalR
        public DeleteModel(DW_26256_27229.Data.ApplicationDbContext context, IHubContext<NotificacaoHub> hubContext) { _context = context; _hubContext = hubContext; }
        [BindProperty]
        public Inscricao Inscricao { get; set; } = default!;

        // Método chamado no carregamento da página para mostrar os dados antes da confirmação
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // Busca a inscrição incluindo os detalhes do Utilizador e do Evento
            var inscricao = await _context.Inscricoes.Include(i => i.Utilizador).Include(i => i.Evento).FirstOrDefaultAsync(m => m.Id == id);
            if (inscricao != null) { Inscricao = inscricao; return Page(); }
            return NotFound();
        }

        // Método chamado quando o utilizador confirma a eliminação no formulário
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao != null)
            {
                Inscricao = inscricao;
                // Remove a inscrição da base de dados
                _context.Inscricoes.Remove(Inscricao);
                await _context.SaveChangesAsync();

                // Calcula a nova lotação do evento após remover a inscrição
                var lotacaoReal = await _context.Inscricoes.CountAsync(i => i.EventoId == inscricao.EventoId);

                // Dispara o SignalR para todos os clientes ativos, enviando o ID do evento e a nova contagem de vagas
                await _hubContext.Clients.All.SendAsync("AtualizarVagas", inscricao.EventoId, lotacaoReal);
            }

            // Redireciona de volta para a listagem
            return RedirectToPage("./Index");
        }
    }
}