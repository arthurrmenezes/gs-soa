using LuzVital.Pontos.Api.Dtos;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Abstractions;
using LuzVital.Pontos.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace LuzVital.Pontos.Api.Controllers;

[ApiController]
[Route("api/pontos")]
[Produces("application/json")]
public class PontosController : ControllerBase
{
    private readonly IServicoPontos _pontos;

    public PontosController(IServicoPontos pontos) => _pontos = pontos;

    /// <summary>Lista os pontos. Todos os filtros são opcionais e podem ser combinados.</summary>
    /// <param name="bairro">Nome do bairro, com ou sem maiúsculas (ex.: Mooca).</param>
    /// <param name="tipo">Tipo de lugar.</param>
    /// <param name="dependencia">Traz só os pontos que servem para quem tem essa dependência.</param>
    /// <param name="somenteDisponiveis">Traz só os pontos que estão funcionando agora.</param>
    /// <param name="ct"></param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PontoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PontoResponse>>> Listar([FromQuery] string? bairro, [FromQuery] TipoPonto? tipo,
        [FromQuery] TipoDependencia? dependencia, [FromQuery] bool somenteDisponiveis = false, CancellationToken ct = default)
    {
        var pontos = await _pontos.ListarAsync(new FiltroPontos(bairro, tipo, dependencia, somenteDisponiveis), ct);
        return Ok(pontos.Select(PontoResponse.De));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PontoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PontoResponse>> Obter(int id, CancellationToken ct) =>
        Ok(PontoResponse.De(await _pontos.ObterAsync(id, ct)));

    /// <summary>Cadastra um ponto. Ele nasce como "Funcionando", com todas as tomadas livres.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PontoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<PontoResponse>> Criar(PontoRequest req, CancellationToken ct)
    {
        var ponto = await _pontos.CriarAsync(req.ParaDados(), ct);
        return CreatedAtAction(nameof(Obter), new { id = ponto.Id }, PontoResponse.De(ponto));
    }

    /// <summary>Corrige o cadastro do ponto. A situação atual não muda por aqui.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PontoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<PontoResponse>> Atualizar(int id, PontoRequest req, CancellationToken ct) =>
        Ok(PontoResponse.De(await _pontos.AtualizarAsync(id, req.ParaDados(), ct)));

    /// <summary>Remove o ponto e o histórico dele.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await _pontos.RemoverAsync(id, ct);
        return NoContent();
    }

    /// <summary>Registra como o ponto está agora (funcionando, lotado, sem energia ou fechado).</summary>
    /// <remarks>
    /// Funcionando: informe quantas tomadas estão livres (sem informar, vale o total).
    /// Fechado: a observação com o motivo é obrigatória.
    /// </remarks>
    [HttpPost("{id:int}/situacoes")]
    [ProducesResponseType(typeof(RegistroSituacaoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegistroSituacaoResponse>> RegistrarSituacao(int id, SituacaoRequest req, CancellationToken ct)
    {
        var registro = await _pontos.RegistrarSituacaoAsync(id, req.Situacao!.Value, req.TomadasLivres, req.Observacao, ct);
        return CreatedAtAction(nameof(Historico), new { id }, RegistroSituacaoResponse.De(registro));
    }

    /// <summary>Histórico de situações do ponto, do mais novo para o mais antigo.</summary>
    [HttpGet("{id:int}/situacoes")]
    [ProducesResponseType(typeof(IEnumerable<RegistroSituacaoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RegistroSituacaoResponse>>> Historico(int id, CancellationToken ct)
    {
        var registros = await _pontos.HistoricoAsync(id, ct);
        return Ok(registros.Select(RegistroSituacaoResponse.De));
    }

    /// <summary>Resumo por bairro: quantos pontos há, quantos têm gerador, quantos estão funcionando e as tomadas livres.</summary>
    [HttpGet("resumo")]
    public async Task<ActionResult<IEnumerable<ResumoBairro>>> Resumo(CancellationToken ct) =>
        Ok(await _pontos.ResumoPorBairroAsync(ct));
}
