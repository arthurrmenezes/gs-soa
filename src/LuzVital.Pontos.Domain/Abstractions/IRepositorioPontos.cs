using LuzVital.Pontos.Domain.Entities;

namespace LuzVital.Pontos.Domain.Abstractions;

public record ResumoBairro(string Bairro, int Pontos, int ComGerador, int Funcionando, int TomadasLivres);

public interface IRepositorioPontos
{
    Task<IReadOnlyList<PontoEnergia>> BuscarAsync(string? bairroNormalizado, TipoPonto? tipo, bool somenteFuncionando, CancellationToken ct = default);

    Task<PontoEnergia?> ObterAsync(int id, CancellationToken ct = default);

    Task<bool> ExisteOutroComMesmoNomeAsync(PontoEnergia ponto, CancellationToken ct = default);

    Task<IReadOnlyList<RegistroSituacao>> HistoricoAsync(int pontoId, CancellationToken ct = default);

    Task<IReadOnlyList<ResumoBairro>> ResumoPorBairroAsync(CancellationToken ct = default);

    void Adicionar(PontoEnergia ponto);

    void Remover(PontoEnergia ponto);

    Task SalvarAsync(CancellationToken ct = default);
}
