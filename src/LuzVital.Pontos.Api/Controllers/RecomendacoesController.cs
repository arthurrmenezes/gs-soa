using System.ComponentModel.DataAnnotations;
using LuzVital.Pontos.Api.Dtos;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace LuzVital.Pontos.Api.Controllers;

[ApiController]
[Route("api/recomendacoes")]
[Produces("application/json")]
public class RecomendacoesController : ControllerBase
{
    private readonly IServicoRecomendacao _recomendacao;

    public RecomendacoesController(IServicoRecomendacao recomendacao) => _recomendacao = recomendacao;

    /// <summary>
    /// Pontos do bairro que estão funcionando e servem para a dependência informada, com mais tomadas livres primeiro,
    /// junto com o alerta mais grave do bairro.
    /// </summary>
    /// <remarks>
    /// O alerta é buscado na API C# principal (<c>GET /api/alertas?bairro=</c>). Se ela não responder,
    /// <c>alertasConsultados</c> vem falso e os pontos são devolvidos do mesmo jeito.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(RecomendacaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RecomendacaoResponse>> Recomendar([FromQuery, Required(ErrorMessage = "Informe o bairro.")] string bairro,
        [FromQuery, Required(ErrorMessage = "Informe a dependência.")] TipoDependencia? dependencia, CancellationToken ct)
    {
        var recomendacao = await _recomendacao.RecomendarAsync(bairro, dependencia!.Value, ct);
        return Ok(RecomendacaoResponse.De(recomendacao));
    }
}
