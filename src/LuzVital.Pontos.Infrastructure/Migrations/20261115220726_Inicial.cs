using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuzVital.Pontos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PontosEnergia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    BairroNormalizado = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Horario = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    TemGerador = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tomadas = table.Column<int>(type: "INTEGER", nullable: false),
                    AceitaEquipamentoMedico = table.Column<bool>(type: "INTEGER", nullable: false),
                    GuardaMedicamento = table.Column<bool>(type: "INTEGER", nullable: false),
                    Acessivel = table.Column<bool>(type: "INTEGER", nullable: false),
                    Situacao = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TomadasLivres = table.Column<int>(type: "INTEGER", nullable: false),
                    SituacaoAtualizadaEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontosEnergia", x => x.Id);
                    table.CheckConstraint("CK_PontosEnergia_EquipamentoMedico", "AceitaEquipamentoMedico = 0 OR (TemGerador = 1 AND Tomadas > 0)");
                    table.CheckConstraint("CK_PontosEnergia_Tomadas", "Tomadas BETWEEN 0 AND 200");
                    table.CheckConstraint("CK_PontosEnergia_TomadasLivres", "TomadasLivres BETWEEN 0 AND Tomadas");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PontosEnergia_BairroNormalizado",
                table: "PontosEnergia",
                column: "BairroNormalizado");

            migrationBuilder.CreateIndex(
                name: "IX_PontosEnergia_Nome_BairroNormalizado",
                table: "PontosEnergia",
                columns: new[] { "Nome", "BairroNormalizado" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PontosEnergia");
        }
    }
}
