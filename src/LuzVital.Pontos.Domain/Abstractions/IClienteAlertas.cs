namespace LuzVital.Pontos.Domain.Abstractions;

public interface IClienteAlertas
{
    Task<ConsultaAlertas> AlertasDoBairroAsync(string bairro, CancellationToken ct = default);
}
