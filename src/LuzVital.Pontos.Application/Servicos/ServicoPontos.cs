using System.Globalization;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Domain.Entities;

namespace LuzVital.Pontos.Application.Servicos;

public record FiltroPontos(string? Bairro = null, TipoPonto? Tipo = null, TipoDependencia? Dependencia = null, bool SomenteDisponiveis = false);

public record DadosPonto(string Nome, TipoPonto Tipo, string Bairro, string Endereco, string Horario, bool TemGerador,
    int Tomadas, bool AceitaEquipamentoMedico, bool GuardaMedicamento, bool Acessivel);

public interface IServicoPontos
{
    Task<IReadOnlyList<PontoEnergia>> ListarAsync(FiltroPontos filtro, CancellationToken ct = default);
    Task<PontoEnergia> ObterAsync(int id, CancellationToken ct = default);
    Task<PontoEnergia> CriarAsync(DadosPonto dados, CancellationToken ct = default);
    Task<PontoEnergia> AtualizarAsync(int id, DadosPonto dados, CancellationToken ct = default);
    Task RemoverAsync(int id, CancellationToken ct = default);
    Task<RegistroSituacao> RegistrarSituacaoAsync(int id, SituacaoPonto situacao, int? tomadasLivres, string? observacao, CancellationToken ct = default);
    Task<IReadOnlyList<RegistroSituacao>> HistoricoAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ResumoBairro>> ResumoPorBairroAsync(CancellationToken ct = default);
}

public class ServicoPontos : IServicoPontos
{
    private static readonly StringComparer OrdemAlfabetica = StringComparer.Create(new CultureInfo("pt-BR"), ignoreCase: true);

    private readonly IRepositorioPontos _repositorio;
    private readonly IRelogio _relogio;

    public ServicoPontos(IRepositorioPontos repositorio, IRelogio relogio)
    {
        _repositorio = repositorio;
        _relogio = relogio;
    }

    public async Task<IReadOnlyList<PontoEnergia>> ListarAsync(FiltroPontos filtro, CancellationToken ct = default)
    {
        var bairro = string.IsNullOrWhiteSpace(filtro.Bairro) ? null : PontoEnergia.NormalizarBairro(filtro.Bairro);
        var pontos = await _repositorio.BuscarAsync(bairro, filtro.Tipo, filtro.SomenteDisponiveis, ct);

        // "Serve para tal dependência" é regra da entidade, então é aplicada aqui, não no banco.
        // A ordenação também: o SQLite não ordena letras acentuadas do jeito do português.
        return pontos
            .Where(p => filtro.Dependencia is not { } dependencia || p.Atende(dependencia))
            .OrderBy(p => p.Bairro, OrdemAlfabetica)
            .ThenBy(p => p.Nome, OrdemAlfabetica)
            .ToList();
    }

    public async Task<PontoEnergia> ObterAsync(int id, CancellationToken ct = default) =>
        await _repositorio.ObterAsync(id, ct) ?? throw NaoEncontradoException.Ponto(id);

    public async Task<PontoEnergia> CriarAsync(DadosPonto d, CancellationToken ct = default)
    {
        var ponto = new PontoEnergia(d.Nome, d.Tipo, d.Bairro, d.Endereco, d.Horario, d.TemGerador, d.Tomadas,
            d.AceitaEquipamentoMedico, d.GuardaMedicamento, d.Acessivel, _relogio.Agora);

        await GarantirNomeLivreAsync(ponto, ct);

        _repositorio.Adicionar(ponto);
        await _repositorio.SalvarAsync(ct);
        return ponto;
    }

    public async Task<PontoEnergia> AtualizarAsync(int id, DadosPonto d, CancellationToken ct = default)
    {
        var ponto = await ObterAsync(id, ct);

        ponto.Atualizar(d.Nome, d.Tipo, d.Bairro, d.Endereco, d.Horario, d.TemGerador, d.Tomadas,
            d.AceitaEquipamentoMedico, d.GuardaMedicamento, d.Acessivel);

        await GarantirNomeLivreAsync(ponto, ct);

        await _repositorio.SalvarAsync(ct);
        return ponto;
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        var ponto = await ObterAsync(id, ct);

        _repositorio.Remover(ponto);
        await _repositorio.SalvarAsync(ct);
    }

    public async Task<RegistroSituacao> RegistrarSituacaoAsync(int id, SituacaoPonto situacao, int? tomadasLivres, string? observacao, CancellationToken ct = default)
    {
        var ponto = await ObterAsync(id, ct);

        var registro = ponto.RegistrarSituacao(situacao, tomadasLivres, observacao, _relogio.Agora);
        await _repositorio.SalvarAsync(ct);
        return registro;
    }

    public async Task<IReadOnlyList<RegistroSituacao>> HistoricoAsync(int id, CancellationToken ct = default)
    {
        await ObterAsync(id, ct);
        return await _repositorio.HistoricoAsync(id, ct);
    }

    public async Task<IReadOnlyList<ResumoBairro>> ResumoPorBairroAsync(CancellationToken ct = default)
    {
        var resumo = await _repositorio.ResumoPorBairroAsync(ct);
        return resumo.OrderBy(r => r.Bairro, OrdemAlfabetica).ToList();
    }

    private async Task GarantirNomeLivreAsync(PontoEnergia ponto, CancellationToken ct)
    {
        if (await _repositorio.ExisteOutroComMesmoNomeAsync(ponto, ct))
            throw new ConflitoException($"Já existe um ponto chamado \"{ponto.Nome}\" em {ponto.Bairro}.");
    }
}
