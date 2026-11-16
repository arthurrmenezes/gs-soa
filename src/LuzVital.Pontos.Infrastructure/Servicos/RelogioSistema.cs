using LuzVital.Pontos.Domain.Abstractions;

namespace LuzVital.Pontos.Infrastructure.Servicos;

public class RelogioSistema : IRelogio
{
    public DateTime Agora => DateTime.UtcNow;
}
