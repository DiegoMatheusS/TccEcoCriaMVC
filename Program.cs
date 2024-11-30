using Microsoft.Extensions.DependencyInjection;
using TccEcoCriaMVC.Message;

var builder = WebApplication.CreateBuilder(args);

// Registra o EmailHelper no container de DI (Dependency Injection)
builder.Services.AddSingleton<EmailHelper>();

// Adiciona suporte ao MVC (Controllers e Views)
builder.Services.AddControllersWithViews();

// Configuração de sessão para controle de tempo de inatividade
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Define o tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Define que o cookie da sessão será acessível apenas via HTTP
    options.Cookie.IsEssential = true; // Marca o cookie como essencial para funcionamento do site
});

// Registra o HttpClient para ser injetado em controladores e serviços
builder.Services.AddHttpClient(); // Adiciona HttpClient para realizar requisições HTTP

// Configurações de ambiente (se não estiver no ambiente de desenvolvimento)
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler(options =>
    {
        options.ExceptionHandlingPath = "/Home/Error"; // Especifica a rota para a página de erro personalizada
    });
}

var app = builder.Build();

// Configura o middleware de requisição/response
// Redireciona para HTTPS e permite o uso de arquivos estáticos
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configura o roteamento de URL
app.UseRouting();

// Configurações para o uso da sessão
app.UseSession();

// Configuração de autenticação e autorização (se necessário)
app.UseAuthentication();
app.UseAuthorization();

// Definição do roteamento para controladores (MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
