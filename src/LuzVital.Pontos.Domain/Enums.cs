namespace LuzVital.Pontos.Domain;

public enum TipoPonto
{
    Saude = 1,
    Biblioteca = 2,
    Metro = 3,
    Comercio = 4,
    Comunidade = 5
}

public enum SituacaoPonto
{
    Funcionando = 1,
    Lotado = 2,
    SemEnergia = 3,
    Fechado = 4
}

public enum TipoDependencia
{
    OxigenioDomiciliar = 1,
    MedicamentoRefrigerado = 2,
    CadeiraMotorizada = 3,
    AcamadoComEquipamento = 4,
    Outro = 99
}

public enum NivelRisco
{
    Baixo = 0,
    Atencao = 1,
    Alto = 2
}
