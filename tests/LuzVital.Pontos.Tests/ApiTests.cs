using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using LuzVital.Pontos.Api;
using LuzVital.Pontos.Api.Dtos;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Infrastructure.Data;
using LuzVital.Pontos.Application.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LuzVital.Pontos.Tests;

public sealed class ApiDeTeste : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _conexao = new("Data Source=:memory:");

    public ConsultaAlertas Alertas { get; set; } = new(true, Array.Empty<AlertaRisco>());

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _conexao.Open();

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<PontosDbContext>>();
            services.AddDbContext<PontosDbContext>(opt => opt.UseSqlite(_conexao));

            services.RemoveAll<IClienteAlertas>();
            services.AddSingleton<IClienteAlertas>(new ClienteAlertasFalso(this));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _conexao.Dispose();
    }

    private sealed class ClienteAlertasFalso : IClienteAlertas
    {
        private readonly ApiDeTeste _api;
        public ClienteAlertasFalso(ApiDeTeste api) => _api = api;
        public Task<ConsultaAlertas> AlertasDoBairroAsync(string bairro, CancellationToken ct = default) => Task.FromResult(_api.Alertas);
    }
}

public sealed class ApiTests : IDisposable
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    private readonly ApiDeTeste _api = new();
    private readonly HttpClient _http;

    public ApiTests() => _http = _api.CreateClient();

    public void Dispose() => _api.Dispose();

    private async Task<T> LerAsync<T>(string caminho) => (await _http.GetFromJsonAsync<T>(caminho, Json))!;

    private static object NovoPonto(string nome = "Centro comunitário de teste", bool temGerador = true, bool aceitaEquipamentoMedico = true) => new
    {
        nome,
        tipo = "Comunidade",
        bairro = "Mooca",
        endereco = "Rua Modelo, 700",
        horario = "Todos os dias, 8h às 22h",
        temGerador,
        tomadas = 12,
        aceitaEquipamentoMedico,
        guardaMedicamento = false,
        acessivel = true
    };

    [Fact]
    public async Task As_migrations_criam_o_banco_com_os_78_pontos_simulados()
    {
        var pontos = await LerAsync<List<PontoResponse>>("/api/pontos");

        Assert.Equal(78, pontos.Count);
        Assert.All(pontos, p => Assert.Equal(SituacaoPonto.Funcionando, p.Situacao));
        Assert.Equal("Aclimação", pontos[0].Bairro);
    }

    [Theory]
    [InlineData("Mooca", 3)]
    [InlineData("mooca", 3)]
    [InlineData("SÃO MATEUS", 2)]
    [InlineData("Bairro que não existe", 0)]
    public async Task Filtra_por_bairro_sem_diferenciar_maiusculas(string bairro, int esperado)
    {
        var pontos = await LerAsync<List<PontoResponse>>($"/api/pontos?bairro={Uri.EscapeDataString(bairro)}");

        Assert.Equal(esperado, pontos.Count);
    }

    [Fact]
    public async Task Filtra_por_tipo_e_por_dependencia()
    {
        var saude = await LerAsync<List<PontoResponse>>("/api/pontos?bairro=Mooca&tipo=Saude");
        var remedio = await LerAsync<List<PontoResponse>>("/api/pontos?bairro=Mooca&dependencia=MedicamentoRefrigerado");

        Assert.Equal("AMA Mooca (simulado)", Assert.Single(saude).Nome);
        Assert.Equal(2, remedio.Count);
        Assert.All(remedio, p => Assert.True(p.GuardaMedicamento));
    }

    [Fact]
    public async Task Filtro_com_valor_invalido_devolve_400()
    {
        var resposta = await _http.GetAsync("/api/pontos?tipo=Padaria");

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    [Fact]
    public async Task Ponto_inexistente_devolve_404()
    {
        Assert.Equal(HttpStatusCode.NotFound, (await _http.GetAsync("/api/pontos/9999")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _http.GetAsync("/api/pontos/9999/situacoes")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _http.DeleteAsync("/api/pontos/9999")).StatusCode);
    }

    [Fact]
    public async Task Cadastra_consulta_atualiza_e_remove_um_ponto()
    {
        var criado = await _http.PostAsJsonAsync("/api/pontos", NovoPonto());
        Assert.Equal(HttpStatusCode.Created, criado.StatusCode);
        var ponto = (await criado.Content.ReadFromJsonAsync<PontoResponse>(Json))!;
        Assert.Equal(12, ponto.TomadasLivres);
        Assert.Equal($"/api/pontos/{ponto.Id}", criado.Headers.Location!.AbsolutePath);

        var atualizado = await _http.PutAsJsonAsync($"/api/pontos/{ponto.Id}", NovoPonto("Centro comunitário renomeado"));
        Assert.Equal(HttpStatusCode.OK, atualizado.StatusCode);
        Assert.Equal("Centro comunitário renomeado", (await LerAsync<PontoResponse>($"/api/pontos/{ponto.Id}")).Nome);

        Assert.Equal(HttpStatusCode.NoContent, (await _http.DeleteAsync($"/api/pontos/{ponto.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _http.GetAsync($"/api/pontos/{ponto.Id}")).StatusCode);
    }

    [Fact]
    public async Task Cadastro_sem_campo_obrigatorio_devolve_400()
    {
        var resposta = await _http.PostAsJsonAsync("/api/pontos", new { nome = "", bairro = "Mooca", tomadas = 500 });

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
        var erro = (await resposta.Content.ReadFromJsonAsync<ErroResponse>(Json))!;
        Assert.Contains("Informe o nome do ponto.", erro.Message);
        Assert.Contains("Informe o tipo do ponto.", erro.Message);
        Assert.Contains("O número de tomadas deve ficar entre 0 e 200.", erro.Message);
    }

    [Fact]
    public async Task Regra_de_negocio_violada_devolve_422_com_a_mensagem()
    {
        var resposta = await _http.PostAsJsonAsync("/api/pontos", NovoPonto(temGerador: false, aceitaEquipamentoMedico: true));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, resposta.StatusCode);
        Assert.Contains("gerador", await resposta.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Ponto_repetido_no_mesmo_bairro_devolve_409()
    {
        var resposta = await _http.PostAsJsonAsync("/api/pontos", NovoPonto("AMA Mooca (simulado)"));

        Assert.Equal(HttpStatusCode.Conflict, resposta.StatusCode);
        Assert.Contains("Já existe", (await resposta.Content.ReadFromJsonAsync<ErroResponse>(Json))!.Message);
    }

    [Fact]
    public async Task Registrar_situacao_muda_o_ponto_guarda_historico_e_tira_da_lista_de_disponiveis()
    {
        const int ama = 4;

        var r1 = await _http.PostAsJsonAsync($"/api/pontos/{ama}/situacoes", new { situacao = "Funcionando", tomadasLivres = 3 });
        var r2 = await _http.PostAsJsonAsync($"/api/pontos/{ama}/situacoes", new { situacao = "Lotado", observacao = "Fila na porta" });
        Assert.Equal(HttpStatusCode.Created, r1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, r2.StatusCode);

        var ponto = await LerAsync<PontoResponse>($"/api/pontos/{ama}");
        Assert.Equal(SituacaoPonto.Lotado, ponto.Situacao);
        Assert.Equal(0, ponto.TomadasLivres);

        var historico = await LerAsync<List<RegistroSituacaoResponse>>($"/api/pontos/{ama}/situacoes");
        Assert.Equal(2, historico.Count);
        Assert.Equal(SituacaoPonto.Lotado, historico[0].Situacao);
        Assert.Equal("Fila na porta", historico[0].Observacao);
        Assert.Equal(3, historico[1].TomadasLivres);

        var disponiveis = await LerAsync<List<PontoResponse>>("/api/pontos?bairro=Mooca&somenteDisponiveis=true");
        Assert.Equal(2, disponiveis.Count);
        Assert.DoesNotContain(disponiveis, p => p.Id == ama);
    }

    [Fact]
    public async Task Situacao_que_fere_a_regra_devolve_422_e_nao_muda_o_ponto()
    {
        var resposta = await _http.PostAsJsonAsync("/api/pontos/4/situacoes", new { situacao = "Funcionando", tomadasLivres = 50 });

        Assert.Equal(HttpStatusCode.UnprocessableEntity, resposta.StatusCode);
        Assert.Equal(8, (await LerAsync<PontoResponse>("/api/pontos/4")).TomadasLivres);
        Assert.Empty(await LerAsync<List<RegistroSituacaoResponse>>("/api/pontos/4/situacoes"));
    }

    [Fact]
    public async Task Remover_o_ponto_apaga_o_historico_junto()
    {
        await _http.PostAsJsonAsync("/api/pontos/4/situacoes", new { situacao = "SemEnergia" });

        Assert.Equal(HttpStatusCode.NoContent, (await _http.DeleteAsync("/api/pontos/4")).StatusCode);

        using var escopo = _api.Services.CreateScope();
        var db = escopo.ServiceProvider.GetRequiredService<PontosDbContext>();
        Assert.Equal(0, await db.Registros.CountAsync());
    }

    [Fact]
    public async Task Resumo_por_bairro_soma_pontos_geradores_e_tomadas_livres()
    {
        await _http.PostAsJsonAsync("/api/pontos/4/situacoes", new { situacao = "SemEnergia" });

        var resumo = await LerAsync<List<ResumoBairro>>("/api/pontos/resumo");

        Assert.Equal(38, resumo.Count);
        Assert.Equal(new ResumoBairro("Mooca", 3, 3, 2, 12), resumo.Single(r => r.Bairro == "Mooca"));
    }

    [Fact]
    public async Task Recomendacao_cruza_o_alerta_da_api_principal_com_os_pontos_que_servem()
    {
        _api.Alertas = new ConsultaAlertas(true, new[]
        {
            new AlertaRisco(1, new DateOnly(2026, 11, 20), NivelRisco.Atencao, "MOOCA", "Vento forte", true),
            new AlertaRisco(2, new DateOnly(2026, 11, 21), NivelRisco.Alto, null, "Temporal", true)
        });
        await _http.PostAsJsonAsync("/api/pontos/4/situacoes", new { situacao = "Funcionando", tomadasLivres = 1 });

        var r = await LerAsync<RecomendacaoResponse>("/api/recomendacoes?bairro=mooca&dependencia=OxigenioDomiciliar");

        Assert.True(r.AlertasConsultados);
        Assert.Equal(2, r.Alerta!.Id);
        Assert.Contains("risco alto", r.Mensagem);
        Assert.Contains("21/11", r.Mensagem);
        Assert.Equal(new[] { "Salão paroquial da Mooca (simulado)", "AMA Mooca (simulado)" }, r.Pontos.Select(p => p.Nome));
    }

    [Fact]
    public async Task Recomendacao_continua_funcionando_quando_a_api_de_alertas_esta_fora_do_ar()
    {
        _api.Alertas = ConsultaAlertas.Indisponivel;

        var r = await LerAsync<RecomendacaoResponse>("/api/recomendacoes?bairro=Mooca&dependencia=MedicamentoRefrigerado");

        Assert.False(r.AlertasConsultados);
        Assert.Null(r.Alerta);
        Assert.Equal(2, r.Pontos.Count());
        Assert.Contains("Não foi possível consultar", r.Mensagem);
    }

    [Fact]
    public async Task Recomendacao_sem_bairro_ou_dependencia_devolve_400()
    {
        Assert.Equal(HttpStatusCode.BadRequest, (await _http.GetAsync("/api/recomendacoes?bairro=Mooca")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await _http.GetAsync("/api/recomendacoes?dependencia=Outro")).StatusCode);
    }

    [Theory]
    [InlineData("GET", "/api/pontos/9999", 404, "Not Found")]
    [InlineData("GET", "/api/pontos?tipo=Padaria", 400, "Bad Request")]
    [InlineData("POST", "/api/pontos/4/situacoes", 422, "Unprocessable Entity")]
    public async Task Todo_erro_segue_o_mesmo_contrato(string metodo, string caminho, int status, string nome)
    {
        var requisicao = new HttpRequestMessage(new HttpMethod(metodo), caminho);
        if (metodo == "POST")
            requisicao.Content = JsonContent.Create(new { situacao = "Fechado" });

        var resposta = await _http.SendAsync(requisicao);
        var erro = (await resposta.Content.ReadFromJsonAsync<ErroResponse>(Json))!;

        Assert.Equal(status, (int)resposta.StatusCode);
        Assert.Equal(status, erro.Status);
        Assert.Equal(nome, erro.Error);
        Assert.False(string.IsNullOrWhiteSpace(erro.Message));
        Assert.Equal(caminho.Split('?')[0], erro.Path);
        Assert.True(erro.Timestamp > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public async Task Swagger_documenta_todos_os_endpoints()
    {
        var doc = await LerAsync<JsonElement>("/swagger/v1/swagger.json");
        var caminhos = doc.GetProperty("paths").EnumerateObject().Select(p => p.Name).ToList();

        Assert.Equal(new[] { "/api/pontos", "/api/pontos/resumo", "/api/pontos/{id}", "/api/pontos/{id}/situacoes", "/api/recomendacoes" },
            caminhos.OrderBy(c => c, StringComparer.Ordinal));
    }
}
