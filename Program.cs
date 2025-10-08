using AppWebCP.Data;
using AppWebCP.Models; // Adicione este using para ter acesso ao modelo Funcionario
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Tempo da sess�o
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===== IN�CIO DO C�DIGO PARA CRIAR USU�RIO =====
// Este bloco ser� executado na inicializa��o do app.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // Garante que o banco de dados est� criado.
        dbContext.Database.EnsureCreated();

        // Verifica se j� existe algum funcion�rio.
        if (!dbContext.Funcionarios.Any())
        {
            // Se n�o existir, cria um novo.
            var adminUser = new Funcionario
            {
                Nome = "Admin",
                Email = "admin@app.com",
                Senha = "admin123" // Lembre-se que a senha est� em texto plano
            };

            // Adiciona o novo usu�rio ao banco de dados e salva.
            dbContext.Funcionarios.Add(adminUser);
            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar o usu�rio inicial no banco de dados.");
    }
}
// ===== FIM DO C�DIGO PARA CRIAR USU�RIO =====


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();