using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuzVital.Pontos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HistoricoDeSituacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosSituacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PontoEnergiaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Situacao = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TomadasLivres = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    RegistradoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosSituacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosSituacao_PontosEnergia_PontoEnergiaId",
                        column: x => x.PontoEnergiaId,
                        principalTable: "PontosEnergia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosSituacao_PontoEnergiaId_RegistradoEm",
                table: "RegistrosSituacao",
                columns: new[] { "PontoEnergiaId", "RegistradoEm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosSituacao");
        }
    }
}
