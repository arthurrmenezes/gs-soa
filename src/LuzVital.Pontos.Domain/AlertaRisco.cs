namespace LuzVital.Pontos.Domain;

public record AlertaRisco(int Id, DateOnly Data, NivelRisco Nivel, string? Bairro, string Mensagem, bool Ativo);

public record ConsultaAlertas(bool Disponivel, IReadOnlyList<AlertaRisco> Alertas)
{
    public static ConsultaAlertas Indisponivel { get; } = new(false, Array.Empty<AlertaRisco>());

    public AlertaRisco? MaisGrave() =>
        Alertas.Where(a => a.Ativo).OrderByDescending(a => a.Nivel).ThenBy(a => a.Data).FirstOrDefault();
}
