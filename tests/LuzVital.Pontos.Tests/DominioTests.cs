using LuzVital.Pontos.Domain;
using LuzVital.Pontos.Domain.Entities;

namespace LuzVital.Pontos.Tests;

public class DominioTests
{
    private static readonly DateTime Agora = new(2026, 11, 15, 12, 0, 0, DateTimeKind.Utc);

    private static PontoEnergia Ubs(int tomadas = 6) =>
        new("UBS de teste", TipoPonto.Saude, " Mooca ", "Rua Modelo, 1", "Aberto 24 horas",
            temGerador: true, tomadas, aceitaEquipamentoMedico: true, guardaMedicamento: true, acessivel: true, Agora);

    private static PontoEnergia Biblioteca() =>
        new("Biblioteca de teste", TipoPonto.Biblioteca, "Mooca", "Rua Modelo, 2", "9h às 18h",
            temGerador: false, tomadas: 10, aceitaEquipamentoMedico: false, guardaMedicamento: false, acessivel: true, Agora);

    [Fact]
    public void Ponto_novo_nasce_funcionando_com_todas_as_tomadas_livres()
    {
        var ponto = Ubs();

        Assert.Equal(SituacaoPonto.Funcionando, ponto.Situacao);
        Assert.Equal(6, ponto.TomadasLivres);
        Assert.True(ponto.PodeReceber);
        Assert.Equal(Agora, ponto.SituacaoAtualizadaEm);
    }

    [Fact]
    public void Bairro_e_guardado_sem_espacos_e_com_versao_em_maiusculas_para_a_busca()
    {
        var ponto = Ubs();

        Assert.Equal("Mooca", ponto.Bairro);
        Assert.Equal("MOOCA", ponto.BairroNormalizado);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Nome_em_branco_e_recusado(string nome)
    {
        var erro = Assert.Throws<DomainException>(() => new PontoEnergia(nome, TipoPonto.Saude, "Mooca", "Rua Modelo, 1",
            "24h", true, 2, false, false, true, Agora));

        Assert.Contains("nome", erro.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(201)]
    public void Numero_de_tomadas_fora_da_faixa_e_recusado(int tomadas)
    {
        Assert.Throws<DomainException>(() => new PontoEnergia("Ponto", TipoPonto.Comercio, "Mooca", "Rua Modelo, 1",
            "24h", true, tomadas, false, false, true, Agora));
    }

    [Theory]
    [InlineData(false, 4)]
    [InlineData(true, 0)]
    public void So_aceita_aparelho_medico_quem_tem_gerador_e_tomada(bool temGerador, int tomadas)
    {
        var erro = Assert.Throws<DomainException>(() => new PontoEnergia("Ponto", TipoPonto.Comercio, "Mooca",
            "Rua Modelo, 1", "24h", temGerador, tomadas, aceitaEquipamentoMedico: true, false, true, Agora));

        Assert.Contains("gerador", erro.Message);
    }

    [Fact]
    public void Registrar_funcionando_atualiza_tomadas_livres_e_entra_no_historico()
    {
        var ponto = Ubs();
        var depois = Agora.AddHours(1);

        var registro = ponto.RegistrarSituacao(SituacaoPonto.Funcionando, 2, "  Movimento alto  ", depois);

        Assert.Equal(2, ponto.TomadasLivres);
        Assert.Equal(depois, ponto.SituacaoAtualizadaEm);
        Assert.Equal("Movimento alto", registro.Observacao);
        Assert.Single(ponto.Registros);
    }

    [Fact]
    public void Funcionando_sem_informar_tomadas_considera_todas_livres()
    {
        var ponto = Ubs();
        ponto.RegistrarSituacao(SituacaoPonto.Lotado, null, null, Agora);

        ponto.RegistrarSituacao(SituacaoPonto.Funcionando, null, null, Agora);

        Assert.Equal(6, ponto.TomadasLivres);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(7)]
    public void Tomadas_livres_nao_passam_do_total_do_ponto(int livres)
    {
        Assert.Throws<DomainException>(() => Ubs().RegistrarSituacao(SituacaoPonto.Funcionando, livres, null, Agora));
    }

    [Fact]
    public void Funcionando_com_zero_tomadas_livres_pede_para_registrar_lotado()
    {
        var erro = Assert.Throws<DomainException>(() => Ubs().RegistrarSituacao(SituacaoPonto.Funcionando, 0, null, Agora));

        Assert.Contains("Lotado", erro.Message);
    }

    [Theory]
    [InlineData(SituacaoPonto.Lotado)]
    [InlineData(SituacaoPonto.SemEnergia)]
    public void Lotado_ou_sem_energia_zera_as_tomadas_livres_e_o_ponto_deixa_de_receber(SituacaoPonto situacao)
    {
        var ponto = Ubs();

        ponto.RegistrarSituacao(situacao, 5, null, Agora);

        Assert.Equal(0, ponto.TomadasLivres);
        Assert.False(ponto.PodeReceber);
    }

    [Fact]
    public void Fechado_exige_o_motivo_na_observacao()
    {
        var ponto = Ubs();

        Assert.Throws<DomainException>(() => ponto.RegistrarSituacao(SituacaoPonto.Fechado, null, " ", Agora));

        ponto.RegistrarSituacao(SituacaoPonto.Fechado, null, "Alagamento na rua", Agora);
        Assert.Equal(SituacaoPonto.Fechado, ponto.Situacao);
    }

    [Fact]
    public void Diminuir_o_total_de_tomadas_no_cadastro_ajusta_as_livres()
    {
        var ponto = Ubs(tomadas: 6);

        ponto.Atualizar("UBS de teste", TipoPonto.Saude, "Mooca", "Rua Modelo, 1", "24h", true, 4, true, true, true);

        Assert.Equal(4, ponto.TomadasLivres);
    }

    [Theory]
    [InlineData(TipoDependencia.OxigenioDomiciliar, true, false)]
    [InlineData(TipoDependencia.MedicamentoRefrigerado, true, false)]
    [InlineData(TipoDependencia.AcamadoComEquipamento, true, false)]
    [InlineData(TipoDependencia.CadeiraMotorizada, true, true)]
    [InlineData(TipoDependencia.Outro, true, true)]
    public void Cada_dependencia_e_atendida_pelo_ponto_certo(TipoDependencia dependencia, bool ubsAtende, bool bibliotecaAtende)
    {
        Assert.Equal(ubsAtende, Ubs().Atende(dependencia));
        Assert.Equal(bibliotecaAtende, Biblioteca().Atende(dependencia));
    }

    [Fact]
    public void O_alerta_mais_grave_vence_e_no_empate_vale_o_dia_mais_proximo()
    {
        var consulta = new ConsultaAlertas(true, new[]
        {
            new AlertaRisco(1, new DateOnly(2026, 11, 16), NivelRisco.Atencao, "MOOCA", "Vento forte", true),
            new AlertaRisco(2, new DateOnly(2026, 11, 20), NivelRisco.Alto, null, "Temporal", true),
            new AlertaRisco(3, new DateOnly(2026, 11, 18), NivelRisco.Alto, null, "Temporal", true),
            new AlertaRisco(4, new DateOnly(2026, 11, 15), NivelRisco.Alto, null, "Já encerrado", false)
        });

        Assert.Equal(3, consulta.MaisGrave()!.Id);
        Assert.Null(ConsultaAlertas.Indisponivel.MaisGrave());
    }
}
