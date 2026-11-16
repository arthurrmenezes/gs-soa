using System.Net;
using System.Text;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Infrastructure.Integracao;
using Microsoft.Extensions.Logging.Abstractions;

namespace LuzVital.Pontos.Tests;

public class ClienteAlertasHttpTests
{
    private sealed class RedeFalsa : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
        public Uri? UltimoEndereco { get; private set; }

        public RedeFalsa(Func<HttpRequestMessage, HttpResponseMessage> responder) => _responder = responder;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            UltimoEndereco = request.RequestUri;
            return Task.FromResult(_responder(request));
        }
    }

    private static ClienteAlertasHttp Cliente(RedeFalsa rede) =>
        new(new HttpClient(rede) { BaseAddress = new Uri("http://api-principal/") }, NullLogger<ClienteAlertasHttp>.Instance);

    [Fact]
    public async Task Le_os_alertas_no_formato_da_api_principal()
    {
        const string json = """
            [
              {"id":7,"data":"2026-11-20","nivel":"Alto","bairro":null,"mensagem":"Risco alto de falta de energia.","ativo":true},
              {"id":8,"data":"2026-11-21","nivel":"Atencao","bairro":"MOOCA","mensagem":"Vento forte.","ativo":true}
            ]
            """;
        var rede = new RedeFalsa(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var consulta = await Cliente(rede).AlertasDoBairroAsync("São Mateus");

        Assert.True(consulta.Disponivel);
        Assert.Equal(2, consulta.Alertas.Count);
        Assert.Equal(new AlertaRisco(7, new DateOnly(2026, 11, 20), NivelRisco.Alto, null, "Risco alto de falta de energia.", true),
            consulta.Alertas[0]);
        Assert.Equal(NivelRisco.Atencao, consulta.Alertas[1].Nivel);
        Assert.Equal("http://api-principal/api/alertas?bairro=S%C3%A3o%20Mateus", rede.UltimoEndereco!.AbsoluteUri);
    }

    [Fact]
    public async Task Api_fora_do_ar_vira_consulta_indisponivel_em_vez_de_erro()
    {
        var rede = new RedeFalsa(_ => throw new HttpRequestException("Conexão recusada"));

        var consulta = await Cliente(rede).AlertasDoBairroAsync("Mooca");

        Assert.False(consulta.Disponivel);
        Assert.Empty(consulta.Alertas);
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, "erro")]
    [InlineData(HttpStatusCode.OK, "isto não é json")]
    public async Task Resposta_com_erro_ou_ilegivel_tambem_vira_indisponivel(HttpStatusCode status, string corpo)
    {
        var rede = new RedeFalsa(_ => new HttpResponseMessage(status)
        {
            Content = new StringContent(corpo, Encoding.UTF8, "application/json")
        });

        var consulta = await Cliente(rede).AlertasDoBairroAsync("Mooca");

        Assert.False(consulta.Disponivel);
    }

    [Fact]
    public async Task Tempo_limite_estourado_vira_indisponivel()
    {
        var rede = new RedeFalsa(_ => throw new TaskCanceledException("tempo limite"));

        var consulta = await Cliente(rede).AlertasDoBairroAsync("Mooca");

        Assert.False(consulta.Disponivel);
    }
}
