using Microsoft.EntityFrameworkCore;
using DW_26256_27229.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Define que a app vai usar SQLite e lê a string de ligação do appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ativa o sistema de login por Cookies. Se o utilizador não estiver autenticado, é redirecionado para a página "/Login"
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Se alguém tentar aceder a uma página bloqueada, é atirado para aqui:
        options.LoginPath = "/Login";
    });

// Ativa os Controladores da API. O IgnoreCycles impede erros quando a base de dados tem ligações circulares (ex: Categoria -> Evento -> Categoria)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Adiciona os serviços base: páginas web, controladores e (SignalR)
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

// Gestão de erros: Se algo correr mal, redireciona para a página de erro personalizada
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Obriga a usar HTTPS para segurança
app.UseHttpsRedirection();

// Ativa o sistema de rotas e verifica quem é o utilizador (Auth) e se tem permissões (Authorization)
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento final: liga as páginas, os controladores e o Hub do SignalR
app.MapStaticAssets();      // Serve ficheiros CSS/JS
app.MapRazorPages()         // Ativa as páginas web
   .WithStaticAssets();
app.MapControllers();       // Ativa a API
app.MapHub<DW_26256_27229.Hubs.NotificacaoHub>("/notificacaoHub"); // O caminho por onde o SignalR vai falar

app.Run();                  // Arranca o servidor