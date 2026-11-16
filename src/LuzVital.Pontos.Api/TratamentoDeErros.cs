using LuzVital.Pontos.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace LuzVital.Pontos.Api;

/// <summary>Contrato único de erro da API: todo 4xx e 5xx responde com este formato.</summary>
/// <param name="Timestamp">Quando o erro aconteceu (UTC).</param>
/// <param name="Status">Código HTTP.</param>
/// <param name="Error">Nome do código HTTP (ex.: Not Found).</param>
/// <param name="Message">O que deu errado, em linguagem simples.</param>
/// <param name="Path">Rota chamada.</param>
public record ErroResponse(DateTime Timestamp, int Status, string Error, string Message, string Path)
{
    public static ErroResponse De(HttpContext http, int status, string mensagem) =>
        new(DateTime.UtcNow, status, ReasonPhrases.GetReasonPhrase(status), mensagem, http.Request.Path);

    public static IActionResult DadoInvalido(ActionContext contexto)
    {
        var mensagens = contexto.ModelState
            .Where(campo => campo.Value?.Errors.Count > 0)
            .SelectMany(campo => campo.Value!.Errors.Select(erro => EhNossa(erro.ErrorMessage)
                ? erro.ErrorMessage
                : $"Valor inválido em '{campo.Key}'."));

        var erro = De(contexto.HttpContext, StatusCodes.Status400BadRequest, string.Join(" ", mensagens.Distinct()));
        return new BadRequestObjectResult(erro);
    }

    // As mensagens escritas por nós, nos DTOs, estão em português; as do framework (JSON ilegível,
    // valor que não cabe no tipo) são técnicas e em inglês, então viram um aviso curto.
    private static bool EhNossa(string mensagem) =>
        mensagem.StartsWith("Informe ") || mensagem.StartsWith("O ") || mensagem.StartsWith("A ") || mensagem.StartsWith("As ");
}

public class FiltroDeErrosDeDominio : IExceptionFilter
{
    public void OnException(ExceptionContext contexto)
    {
        if (contexto.Exception is not DomainException ex)
            return;

        var status = ex switch
        {
            NaoEncontradoException => StatusCodes.Status404NotFound,
            ConflitoException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status422UnprocessableEntity
        };

        contexto.Result = new ObjectResult(ErroResponse.De(contexto.HttpContext, status, ex.Message)) { StatusCode = status };
        contexto.ExceptionHandled = true;
    }
}

public class TratamentoDeErros : IExceptionHandler
{
    private readonly ILogger<TratamentoDeErros> _logger;

    public TratamentoDeErros(ILogger<TratamentoDeErros> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception ex, CancellationToken ct)
    {
        _logger.LogError(ex, "Erro não tratado em {Path}", http.Request.Path);

        http.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await http.Response.WriteAsJsonAsync(
            ErroResponse.De(http, StatusCodes.Status500InternalServerError, "Erro inesperado. Tente de novo."), ct);
        return true;
    }
}
