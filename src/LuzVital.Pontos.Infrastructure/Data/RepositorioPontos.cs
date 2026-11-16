using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuzVital.Pontos.Infrastructure.Data;

public class RepositorioPontos : IRepositorioPontos
{
    private readonly PontosDbContext _db;

    public RepositorioPontos(PontosDbContext db) => _db = db;

    public async Task<IReadOnlyList<PontoEnergia>> BuscarAsync(string? bairroNormalizado, TipoPonto? tipo, bool somenteFuncionando, CancellationToken ct = default)
    {
        var query = _db.Pontos.AsNoTracking();

        if (bairroNormalizado is not null)
            query = query.Where(p => p.BairroNormalizado == bairroNormalizado);
        if (tipo is not null)
            query = query.Where(p => p.Tipo == tipo);
        if (somenteFuncionando)
            query = query.Where(p => p.Situacao == SituacaoPonto.Funcionando);

        return await query.ToListAsync(ct);
    }

    public Task<PontoEnergia?> ObterAsync(int id, CancellationToken ct = default) =>
        _db.Pontos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> ExisteOutroComMesmoNomeAsync(PontoEnergia ponto, CancellationToken ct = default) =>
        _db.Pontos.AnyAsync(p => p.Id != ponto.Id && p.Nome == ponto.Nome && p.BairroNormalizado == ponto.BairroNormalizado, ct);

    public async Task<IReadOnlyList<RegistroSituacao>> HistoricoAsync(int pontoId, CancellationToken ct = default) =>
        await _db.Registros.AsNoTracking()
            .Where(r => r.PontoEnergiaId == pontoId)
            .OrderByDescending(r => r.RegistradoEm).ThenByDescending(r => r.Id)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ResumoBairro>> ResumoPorBairroAsync(CancellationToken ct = default) =>
        await _db.Pontos.AsNoTracking()
            .GroupBy(p => p.Bairro)
            .Select(g => new ResumoBairro(
                g.Key,
                g.Count(),
                g.Count(p => p.TemGerador),
                g.Count(p => p.Situacao == SituacaoPonto.Funcionando),
                g.Sum(p => p.TomadasLivres)))
            .ToListAsync(ct);

    public void Adicionar(PontoEnergia ponto) => _db.Pontos.Add(ponto);

    public void Remover(PontoEnergia ponto) => _db.Pontos.Remove(ponto);

    public Task SalvarAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
