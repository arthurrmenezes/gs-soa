using System.ComponentModel.DataAnnotations;
using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Entities;
using LuzVital.Pontos.Application.Servicos;

namespace LuzVital.Pontos.Api.Dtos;

public record PontoRequest(
    [Required(ErrorMessage = "Informe o nome do ponto."), StringLength(120, ErrorMessage = "O nome pode ter até 120 letras.")] string Nome,
    [Required(ErrorMessage = "Informe o tipo do ponto.")] TipoPonto? Tipo,
    [Required(ErrorMessage = "Informe o bairro."), StringLength(80, ErrorMessage = "O bairro pode ter até 80 letras.")] string Bairro,
    [Required(ErrorMessage = "Informe o endereço."), StringLength(160, ErrorMessage = "O endereço pode ter até 160 letras.")] string Endereco,
    [Required(ErrorMessage = "Informe o horário."), StringLength(80, ErrorMessage = "O horário pode ter até 80 letras.")] string Horario,
    bool TemGerador,
    [Range(0, PontoEnergia.MaximoTomadas, ErrorMessage = "O número de tomadas deve ficar entre 0 e 200.")] int Tomadas,
    bool AceitaEquipamentoMedico,
    bool GuardaMedicamento,
    bool Acessivel)
{
    public DadosPonto ParaDados() => new(Nome, Tipo!.Value, Bairro, Endereco, Horario, TemGerador, Tomadas,
        AceitaEquipamentoMedico, GuardaMedicamento, Acessivel);
}

public record PontoResponse(int Id, string Nome, TipoPonto Tipo, string Bairro, string Endereco, string Horario,
    bool TemGerador, int Tomadas, bool AceitaEquipamentoMedico, bool GuardaMedicamento, bool Acessivel,
    SituacaoPonto Situacao, int TomadasLivres, DateTime SituacaoAtualizadaEm)
{
    public static PontoResponse De(PontoEnergia p) => new(p.Id, p.Nome, p.Tipo, p.Bairro, p.Endereco, p.Horario,
        p.TemGerador, p.Tomadas, p.AceitaEquipamentoMedico, p.GuardaMedicamento, p.Acessivel,
        p.Situacao, p.TomadasLivres, p.SituacaoAtualizadaEm);
}

public record SituacaoRequest(
    [Required(ErrorMessage = "Informe a situação.")] SituacaoPonto? Situacao,
    [Range(0, PontoEnergia.MaximoTomadas, ErrorMessage = "As tomadas livres devem ficar entre 0 e 200.")] int? TomadasLivres,
    [StringLength(RegistroSituacao.TamanhoMaximoObservacao, ErrorMessage = "A observação pode ter até 300 letras.")] string? Observacao);

public record RegistroSituacaoResponse(int Id, int PontoId, SituacaoPonto Situacao, int TomadasLivres, string? Observacao, DateTime RegistradoEm)
{
    public static RegistroSituacaoResponse De(RegistroSituacao r) =>
        new(r.Id, r.PontoEnergiaId, r.Situacao, r.TomadasLivres, r.Observacao, r.RegistradoEm);
}

public record RecomendacaoResponse(string Bairro, TipoDependencia Dependencia, bool AlertasConsultados, AlertaRisco? Alerta,
    string Mensagem, IEnumerable<PontoResponse> Pontos)
{
    public static RecomendacaoResponse De(Recomendacao r) =>
        new(r.Bairro, r.Dependencia, r.AlertasConsultados, r.Alerta, r.Mensagem, r.Pontos.Select(PontoResponse.De));
}
