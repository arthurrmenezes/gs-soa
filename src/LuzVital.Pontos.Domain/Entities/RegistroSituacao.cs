namespace LuzVital.Pontos.Domain.Entities;

public class RegistroSituacao
{
    public const int TamanhoMaximoObservacao = 300;

    public int Id { get; private set; }
    public int PontoEnergiaId { get; private set; }
    public PontoEnergia Ponto { get; private set; } = null!;
    public SituacaoPonto Situacao { get; private set; }
    public int TomadasLivres { get; private set; }
    public string? Observacao { get; private set; }
    public DateTime RegistradoEm { get; private set; }

    private RegistroSituacao() { }

    internal RegistroSituacao(PontoEnergia ponto, SituacaoPonto situacao, int tomadasLivres, string? observacao, DateTime registradoEm)
    {
        Ponto = ponto;
        Situacao = situacao;
        TomadasLivres = tomadasLivres;
        Observacao = observacao;
        RegistradoEm = registradoEm;
    }
}
