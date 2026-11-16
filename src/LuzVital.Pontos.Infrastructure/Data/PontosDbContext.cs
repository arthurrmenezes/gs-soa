using LuzVital.Pontos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LuzVital.Pontos.Infrastructure.Data;

public class PontosDbContext : DbContext
{
    public PontosDbContext(DbContextOptions<PontosDbContext> options) : base(options) { }

    public DbSet<PontoEnergia> Pontos => Set<PontoEnergia>();
    public DbSet<RegistroSituacao> Registros => Set<RegistroSituacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // O SQLite guarda a data como texto e perde o "é UTC" na leitura; o conversor devolve.
        var utc = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        modelBuilder.Entity<PontoEnergia>(e =>
        {
            e.ToTable("PontosEnergia", t =>
            {
                t.HasCheckConstraint("CK_PontosEnergia_Tomadas", "Tomadas BETWEEN 0 AND 200");
                t.HasCheckConstraint("CK_PontosEnergia_TomadasLivres", "TomadasLivres BETWEEN 0 AND Tomadas");
                t.HasCheckConstraint("CK_PontosEnergia_EquipamentoMedico",
                    "AceitaEquipamentoMedico = 0 OR (TemGerador = 1 AND Tomadas > 0)");
            });

            e.Property(p => p.Nome).HasMaxLength(120);
            e.Property(p => p.Bairro).HasMaxLength(80);
            e.Property(p => p.BairroNormalizado).HasMaxLength(80);
            e.Property(p => p.Endereco).HasMaxLength(160);
            e.Property(p => p.Horario).HasMaxLength(80);
            e.Property(p => p.Tipo).HasConversion<string>().HasMaxLength(20);
            e.Property(p => p.Situacao).HasConversion<string>().HasMaxLength(20);
            e.Property(p => p.SituacaoAtualizadaEm).HasConversion(utc);

            e.HasIndex(p => p.BairroNormalizado);
            e.HasIndex(p => new { p.Nome, p.BairroNormalizado }).IsUnique();

            e.HasMany(p => p.Registros)
                .WithOne(r => r.Ponto)
                .HasForeignKey(r => r.PontoEnergiaId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Navigation(p => p.Registros).UsePropertyAccessMode(PropertyAccessMode.Field);

            e.HasData(PontosSimulados.Todos);
        });

        modelBuilder.Entity<RegistroSituacao>(e =>
        {
            e.ToTable("RegistrosSituacao");
            e.Property(r => r.Situacao).HasConversion<string>().HasMaxLength(20);
            e.Property(r => r.Observacao).HasMaxLength(RegistroSituacao.TamanhoMaximoObservacao);
            e.Property(r => r.RegistradoEm).HasConversion(utc);
            e.HasIndex(r => new { r.PontoEnergiaId, r.RegistradoEm });
        });
    }
}

public class PontosDbContextFactory : IDesignTimeDbContextFactory<PontosDbContext>
{
    public PontosDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<PontosDbContext>().UseSqlite("Data Source=luzvital-pontos.db").Options);
}
