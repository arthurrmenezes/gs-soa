using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace LuzVital.Pontos.Infrastructure.Integracao;

public class ClienteAlertasHttp : IClienteAlertas
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _http;
    private readonly ILogger<ClienteAlertasHttp> _logger;

    public ClienteAlertasHttp(HttpClient http, ILogger<ClienteAlertasHttp> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<ConsultaAlertas> AlertasDoBairroAsync(string bairro, CancellationToken ct = default)
    {
        try
        {
            var alertas = await _http.GetFromJsonAsync<List<AlertaRisco>>(
                $"api/alertas?bairro={Uri.EscapeDataString(bairro)}", Json, ct);

            return new ConsultaAlertas(true, alertas ?? new List<AlertaRisco>());
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException
                                   || (ex is OperationCanceledException && !ct.IsCancellationRequested))
        {
            _logger.LogWarning("A API de alertas não respondeu ({Motivo}). Seguindo sem o alerta.", ex.Message);
            return ConsultaAlertas.Indisponivel;
        }
    }
}
