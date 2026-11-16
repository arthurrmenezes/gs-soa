namespace LuzVital.Pontos.Domain.Entities;

public class PontoEnergia
{
    public const int MaximoTomadas = 200;

    private readonly List<RegistroSituacao> _registros = new();

    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public TipoPonto Tipo { get; private set; }
    public string Bairro { get; private set; } = null!;
    public string BairroNormalizado { get; private set; } = null!;
    public string Endereco { get; private set; } = null!;
    public string Horario { get; private set; } = null!;
    public bool TemGerador { get; private set; }
    public int Tomadas { get; private set; }
    public bool AceitaEquipamentoMedico { get; private set; }
    public bool GuardaMedicamento { get; private set; }
    public bool Acessivel { get; private set; }

    public SituacaoPonto Situacao { get; private set; }
    public int TomadasLivres { get; private set; }
    public DateTime SituacaoAtualizadaEm { get; private set; }

    public IReadOnlyCollection<RegistroSituacao> Registros => _registros;

    private PontoEnergia() { }

    public PontoEnergia(string nome, TipoPonto tipo, string bairro, string endereco, string horario, bool temGerador,
        int tomadas, bool aceitaEquipamentoMedico, bool guardaMedicamento, bool acessivel, DateTime agora)
    {
        Definir(nome, tipo, bairro, endereco, horario, temGerador, tomadas, aceitaEquipamentoMedico, guardaMedicamento, acessivel);
        Situacao = SituacaoPonto.Funcionando;
        TomadasLivres = Tomadas;
        SituacaoAtualizadaEm = agora;
    }

    public void Atualizar(string nome, TipoPonto tipo, string bairro, string endereco, string horario, bool temGerador,
        int tomadas, bool aceitaEquipamentoMedico, bool guardaMedicamento, bool acessivel)
    {
        Definir(nome, tipo, bairro, endereco, horario, temGerador, tomadas, aceitaEquipamentoMedico, guardaMedicamento, acessivel);
        TomadasLivres = Math.Min(TomadasLivres, Tomadas);
    }

    public RegistroSituacao RegistrarSituacao(SituacaoPonto situacao, int? tomadasLivres, string? observacao, DateTime agora)
    {
        if (!Enum.IsDefined(situacao))
            throw new DomainException("Situação inválida.");

        observacao = string.IsNullOrWhiteSpace(observacao) ? null : observacao.Trim();
        if (observacao is { Length: > RegistroSituacao.TamanhoMaximoObservacao })
            throw new DomainException($"A observação pode ter até {RegistroSituacao.TamanhoMaximoObservacao} letras.");

        int livres;
        if (situacao == SituacaoPonto.Funcionando)
        {
            livres = tomadasLivres ?? Tomadas;
            if (livres < 0 || livres > Tomadas)
                throw new DomainException($"As tomadas livres devem ficar entre 0 e {Tomadas}, que é o total do ponto.");
            if (livres == 0 && Tomadas > 0)
                throw new DomainException("Sem tomada livre, registre a situação Lotado.");
        }
        else
        {
            if (situacao == SituacaoPonto.Fechado && observacao is null)
                throw new DomainException("Para registrar o ponto como fechado, diga o motivo na observação.");
            livres = 0;
        }

        Situacao = situacao;
        TomadasLivres = livres;
        SituacaoAtualizadaEm = agora;

        var registro = new RegistroSituacao(this, situacao, livres, observacao, agora);
        _registros.Add(registro);
        return registro;
    }

    public bool PodeReceber => Situacao == SituacaoPonto.Funcionando;

    public bool Atende(TipoDependencia dependencia) => dependencia switch
    {
        TipoDependencia.OxigenioDomiciliar => AceitaEquipamentoMedico,
        TipoDependencia.MedicamentoRefrigerado => GuardaMedicamento,
        TipoDependencia.CadeiraMotorizada => Acessivel && Tomadas > 0,
        TipoDependencia.AcamadoComEquipamento => AceitaEquipamentoMedico && Acessivel,
        _ => Tomadas > 0
    };

    public static string NormalizarBairro(string bairro) => bairro.Trim().ToUpperInvariant();

    private void Definir(string nome, TipoPonto tipo, string bairro, string endereco, string horario, bool temGerador,
        int tomadas, bool aceitaEquipamentoMedico, bool guardaMedicamento, bool acessivel)
    {
        if (!Enum.IsDefined(tipo))
            throw new DomainException("Tipo de ponto inválido.");
        if (tomadas < 0 || tomadas > MaximoTomadas)
            throw new DomainException($"O número de tomadas deve ficar entre 0 e {MaximoTomadas}.");
        if (aceitaEquipamentoMedico && (!temGerador || tomadas == 0))
            throw new DomainException("Só aceita aparelho médico o ponto que tem gerador e pelo menos uma tomada.");

        Nome = Texto(nome, "nome", 120);
        Tipo = tipo;
        Bairro = Texto(bairro, "bairro", 80);
        BairroNormalizado = NormalizarBairro(Bairro);
        Endereco = Texto(endereco, "endereço", 160);
        Horario = Texto(horario, "horário", 80);
        TemGerador = temGerador;
        Tomadas = tomadas;
        AceitaEquipamentoMedico = aceitaEquipamentoMedico;
        GuardaMedicamento = guardaMedicamento;
        Acessivel = acessivel;
    }

    private static string Texto(string? valor, string campo, int maximo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException($"Informe o {campo} do ponto.");
        valor = valor.Trim();
        if (valor.Length > maximo)
            throw new DomainException($"O {campo} pode ter até {maximo} letras.");
        return valor;
    }
}
