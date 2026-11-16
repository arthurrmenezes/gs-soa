using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Domain.Entities;

namespace LuzVital.Pontos.Application.Servicos;

public record Recomendacao(string Bairro, TipoDependencia Dependencia, bool AlertasConsultados, AlertaRisco? Alerta,
    string Mensagem, IReadOnlyList<PontoEnergia> Pontos);

public interface IServicoRecomendacao
{
    Task<Recomendacao> RecomendarAsync(string bairro, TipoDependencia dependencia, CancellationToken ct = default);
}

public class ServicoRecomendacao : IServicoRecomendacao
{
    private readonly IServicoPontos _pontos;
    private readonly IClienteAlertas _alertas;

    public ServicoRecomendacao(IServicoPontos pontos, IClienteAlertas alertas)
    {
        _pontos = pontos;
        _alertas = alertas;
    }

    public async Task<Recomendacao> RecomendarAsync(string bairro, TipoDependencia dependencia, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(bairro))
            throw new DomainException("Informe o bairro.");
        if (!Enum.IsDefined(dependencia))
            throw new DomainException("Dependência inválida.");

        bairro = bairro.Trim();

        var consulta = await _alertas.AlertasDoBairroAsync(bairro, ct);
        var alerta = consulta.MaisGrave();

        var pontos = await _pontos.ListarAsync(new FiltroPontos(bairro, Dependencia: dependencia, SomenteDisponiveis: true), ct);
        var ordenados = pontos.OrderByDescending(p => p.TomadasLivres).ToList();

        return new Recomendacao(bairro, dependencia, consulta.Disponivel, alerta, Mensagem(consulta, alerta, ordenados.Count), ordenados);
    }

    private static string Mensagem(ConsultaAlertas consulta, AlertaRisco? alerta, int quantidade)
    {
        var aviso = !consulta.Disponivel
            ? "Não foi possível consultar os avisos de risco agora."
            : alerta is null
                ? "Não há aviso de risco para o seu bairro no momento."
                : $"Há aviso de risco {Rotulo(alerta.Nivel)} de falta de energia para {alerta.Data:dd/MM}.";

        var lugares = quantidade switch
        {
            0 => "Nenhum lugar do bairro atende a sua necessidade agora. Veja os lugares dos bairros vizinhos.",
            1 => "Há 1 lugar no bairro que atende a sua necessidade.",
            _ => $"Há {quantidade} lugares no bairro que atendem a sua necessidade."
        };

        return $"{aviso} {lugares}";
    }

    private static string Rotulo(NivelRisco nivel) => nivel switch
    {
        NivelRisco.Alto => "alto",
        NivelRisco.Atencao => "moderado",
        _ => "baixo"
    };
}
