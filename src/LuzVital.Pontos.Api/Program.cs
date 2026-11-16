using System.Reflection;
using System.Text.Json.Serialization;
using LuzVital.Pontos.Api;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Infrastructure.Data;
using LuzVital.Pontos.Infrastructure.Integracao;
using LuzVital.Pontos.Application.Servicos;
using LuzVital.Pontos.Infrastructure.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PontosDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Pontos") ?? "Data Source=luzvital-pontos.db"));

builder.Services.AddScoped<IRepositorioPontos, RepositorioPontos>();
builder.Services.AddScoped<IServicoPontos, ServicoPontos>();
builder.Services.AddScoped<IServicoRecomendacao, ServicoRecomendacao>();
builder.Services.AddSingleton<IRelogio, RelogioSistema>();

builder.Services.AddHttpClient<IClienteAlertas, ClienteAlertasHttp>(http =>
{
    var url = builder.Configuration["ApiAlertas:Url"] ?? "http://localhost:5131";
    http.BaseAddress = new Uri(url.TrimEnd('/') + "/");
    http.Timeout = TimeSpan.FromSeconds(builder.Configuration.GetValue("ApiAlertas:TempoLimiteSegundos", 3));
});

builder.Services.AddExceptionHandler<TratamentoDeErros>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers(o => o.Filters.Add<FiltroDeErrosDeDominio>())
    .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = ErroResponse.DadoInvalido)
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Luz Vital - Pontos com energia",
        Version = "v1",
        Description = "Serviço de pontos com energia da Luz Vital: lugares onde quem depende de energia em casa pode ir " +
                      "durante um apagão. Global Solution CitizenTech, FIAP 2026. Todos os lugares e endereços são fictícios."
    });

    foreach (var assembly in new[] { Assembly.GetExecutingAssembly(), typeof(LuzVital.Pontos.Domain.TipoPonto).Assembly })
    {
        var xml = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (File.Exists(xml))
            c.IncludeXmlComments(xml);
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PontosDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c => c.DocumentTitle = "Luz Vital - Pontos com energia");

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();

public partial class Program { }
