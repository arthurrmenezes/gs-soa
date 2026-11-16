using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LuzVital.Pontos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PontosSimulados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PontosEnergia",
                columns: new[] { "Id", "AceitaEquipamentoMedico", "Acessivel", "Bairro", "BairroNormalizado", "Endereco", "GuardaMedicamento", "Horario", "Nome", "Situacao", "SituacaoAtualizadaEm", "TemGerador", "Tipo", "Tomadas", "TomadasLivres" },
                values: new object[,]
                {
                    { 1, true, true, "Bela Vista", "BELA VISTA", "Rua Exemplo, 300", true, "Todos os dias, 7h às 19h", "UBS Bela Vista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 2, false, true, "Bela Vista", "BELA VISTA", "Rua Exemplo, 512", false, "Segunda a sábado, 9h às 18h", "Biblioteca do Bixiga (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 12, 12 },
                    { 3, false, true, "Bela Vista", "BELA VISTA", "Avenida Exemplo, 1000", false, "Todos os dias, 4h40 à meia-noite", "Estação de metrô Bela Vista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Metro", 4, 4 },
                    { 4, true, true, "Mooca", "MOOCA", "Rua Modelo, 120", true, "Aberto 24 horas", "AMA Mooca (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 8, 8 },
                    { 5, false, false, "Mooca", "MOOCA", "Rua Modelo, 455", true, "Todos os dias, 7h às 23h", "Farmácia parceira da Mooca (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 6, true, true, "Mooca", "MOOCA", "Praça Modelo, 10", false, "Abre quando há aviso de risco", "Salão paroquial da Mooca (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 10, 10 },
                    { 7, true, true, "Liberdade", "LIBERDADE", "Rua Amostra, 80", true, "Segunda a sexta, 7h às 19h", "UBS Liberdade (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 8, false, true, "Liberdade", "LIBERDADE", "Rua Amostra, 233", false, "Terça a domingo, 10h às 20h", "Centro cultural da Liberdade (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Comunidade", 14, 14 },
                    { 9, true, true, "Tatuapé", "TATUAPÉ", "Rua Teste, 900", true, "Aberto 24 horas", "Hospital dia do Tatuapé (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 10, 10 },
                    { 10, false, true, "Tatuapé", "TATUAPÉ", "Rua Teste, 15", false, "Todos os dias, 4h40 à meia-noite", "Estação de metrô Tatuapé (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Metro", 6, 6 },
                    { 11, true, true, "Santana", "SANTANA", "Rua Ensaio, 64", true, "Segunda a sexta, 7h às 19h", "UBS Santana (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 12, false, false, "Santana", "SANTANA", "Avenida Ensaio, 700", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Santana (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 9, 9 },
                    { 13, true, true, "Pinheiros", "PINHEIROS", "Rua Protótipo, 41", true, "Segunda a sexta, 7h às 19h", "UBS Pinheiros (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 14, false, true, "Pinheiros", "PINHEIROS", "Rua Protótipo, 388", true, "Todos os dias, 8h às 22h", "Mercado parceiro de Pinheiros (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 3, 3 },
                    { 15, true, true, "Aclimação", "ACLIMAÇÃO", "Rua Exemplo, 205", true, "Aberto 24 horas", "UBS Aclimação (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 16, false, true, "Aclimação", "ACLIMAÇÃO", "Rua Modelo, 130", false, "Segunda a sábado, 9h às 18h", "Biblioteca da Aclimação (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 17, true, true, "Belém", "BELÉM", "Rua Exemplo, 219", true, "Segunda a sexta, 7h às 19h", "UBS Belém (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 18, false, true, "Belém", "BELÉM", "Rua Modelo, 140", true, "Todos os dias, 7h às 23h", "Farmácia parceira do Belém (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 19, true, true, "Bom Retiro", "BOM RETIRO", "Rua Exemplo, 233", true, "Segunda a sexta, 7h às 19h", "UBS Bom Retiro (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 20, true, false, "Bom Retiro", "BOM RETIRO", "Rua Modelo, 150", false, "Abre quando há aviso de risco", "Centro comunitário do Bom Retiro (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 21, true, true, "Brás", "BRÁS", "Rua Exemplo, 247", true, "Segunda a sexta, 7h às 19h", "UBS Brás (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 22, false, true, "Brás", "BRÁS", "Rua Modelo, 160", false, "Segunda a sábado, 9h às 18h", "Biblioteca do Brás (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 23, true, true, "Butantã", "BUTANTÃ", "Rua Exemplo, 261", true, "Aberto 24 horas", "UBS Butantã (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 24, false, true, "Butantã", "BUTANTÃ", "Rua Modelo, 170", true, "Todos os dias, 7h às 23h", "Farmácia parceira do Butantã (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 25, true, true, "Cambuci", "CAMBUCI", "Rua Exemplo, 275", true, "Segunda a sexta, 7h às 19h", "UBS Cambuci (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 26, true, false, "Cambuci", "CAMBUCI", "Rua Modelo, 180", false, "Abre quando há aviso de risco", "Centro comunitário do Cambuci (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 27, true, true, "Campo Belo", "CAMPO BELO", "Rua Exemplo, 289", true, "Segunda a sexta, 7h às 19h", "UBS Campo Belo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 28, false, true, "Campo Belo", "CAMPO BELO", "Rua Modelo, 190", false, "Segunda a sábado, 9h às 18h", "Biblioteca do Campo Belo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 29, true, true, "Campo Limpo", "CAMPO LIMPO", "Rua Exemplo, 303", true, "Segunda a sexta, 7h às 19h", "UBS Campo Limpo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 30, false, true, "Campo Limpo", "CAMPO LIMPO", "Rua Modelo, 200", true, "Todos os dias, 7h às 23h", "Farmácia parceira do Campo Limpo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 31, true, true, "Capão Redondo", "CAPÃO REDONDO", "Rua Exemplo, 317", true, "Aberto 24 horas", "UBS Capão Redondo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 32, true, false, "Capão Redondo", "CAPÃO REDONDO", "Rua Modelo, 210", false, "Abre quando há aviso de risco", "Centro comunitário do Capão Redondo (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 33, true, true, "Casa Verde", "CASA VERDE", "Rua Exemplo, 331", true, "Segunda a sexta, 7h às 19h", "UBS Casa Verde (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 34, false, true, "Casa Verde", "CASA VERDE", "Rua Modelo, 220", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Casa Verde (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 35, true, true, "Consolação", "CONSOLAÇÃO", "Rua Exemplo, 345", true, "Segunda a sexta, 7h às 19h", "UBS Consolação (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 36, false, true, "Consolação", "CONSOLAÇÃO", "Rua Modelo, 230", true, "Todos os dias, 7h às 23h", "Farmácia parceira da Consolação (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 37, true, true, "Freguesia do Ó", "FREGUESIA DO Ó", "Rua Exemplo, 359", true, "Segunda a sexta, 7h às 19h", "UBS Freguesia do Ó (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 38, true, false, "Freguesia do Ó", "FREGUESIA DO Ó", "Rua Modelo, 240", false, "Abre quando há aviso de risco", "Centro comunitário da Freguesia do Ó (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 39, true, true, "Grajaú", "GRAJAÚ", "Rua Exemplo, 373", true, "Aberto 24 horas", "UBS Grajaú (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 40, false, true, "Grajaú", "GRAJAÚ", "Rua Modelo, 250", false, "Segunda a sábado, 9h às 18h", "Biblioteca do Grajaú (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 41, true, true, "Ipiranga", "IPIRANGA", "Rua Exemplo, 387", true, "Segunda a sexta, 7h às 19h", "UBS Ipiranga (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 42, false, true, "Ipiranga", "IPIRANGA", "Rua Modelo, 260", true, "Todos os dias, 7h às 23h", "Farmácia parceira do Ipiranga (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 43, true, true, "Itaim Bibi", "ITAIM BIBI", "Rua Exemplo, 401", true, "Segunda a sexta, 7h às 19h", "UBS Itaim Bibi (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 44, true, false, "Itaim Bibi", "ITAIM BIBI", "Rua Modelo, 270", false, "Abre quando há aviso de risco", "Centro comunitário do Itaim Bibi (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 45, true, true, "Itaquera", "ITAQUERA", "Rua Exemplo, 415", true, "Segunda a sexta, 7h às 19h", "UBS Itaquera (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 46, false, true, "Itaquera", "ITAQUERA", "Rua Modelo, 280", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Itaquera (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 47, true, true, "Jabaquara", "JABAQUARA", "Rua Exemplo, 429", true, "Aberto 24 horas", "UBS Jabaquara (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 48, false, true, "Jabaquara", "JABAQUARA", "Rua Modelo, 290", true, "Todos os dias, 7h às 23h", "Farmácia parceira do Jabaquara (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 49, true, true, "Jardim Paulista", "JARDIM PAULISTA", "Rua Exemplo, 443", true, "Segunda a sexta, 7h às 19h", "UBS Jardim Paulista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 50, true, false, "Jardim Paulista", "JARDIM PAULISTA", "Rua Modelo, 300", false, "Abre quando há aviso de risco", "Centro comunitário do Jardim Paulista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 51, true, true, "Lapa", "LAPA", "Rua Exemplo, 457", true, "Segunda a sexta, 7h às 19h", "UBS Lapa (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 52, false, true, "Lapa", "LAPA", "Rua Modelo, 310", false, "Segunda a sábado, 9h às 18h", "Biblioteca da Lapa (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 53, true, true, "Moema", "MOEMA", "Rua Exemplo, 471", true, "Segunda a sexta, 7h às 19h", "UBS Moema (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 54, false, true, "Moema", "MOEMA", "Rua Modelo, 320", true, "Todos os dias, 7h às 23h", "Farmácia parceira de Moema (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 55, true, true, "Penha", "PENHA", "Rua Exemplo, 485", true, "Aberto 24 horas", "UBS Penha (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 56, true, false, "Penha", "PENHA", "Rua Modelo, 330", false, "Abre quando há aviso de risco", "Centro comunitário da Penha (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 57, true, true, "Perdizes", "PERDIZES", "Rua Exemplo, 499", true, "Segunda a sexta, 7h às 19h", "UBS Perdizes (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 58, false, true, "Perdizes", "PERDIZES", "Rua Modelo, 340", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Perdizes (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 59, true, true, "Pirituba", "PIRITUBA", "Rua Exemplo, 513", true, "Segunda a sexta, 7h às 19h", "UBS Pirituba (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 60, false, true, "Pirituba", "PIRITUBA", "Rua Modelo, 350", true, "Todos os dias, 7h às 23h", "Farmácia parceira de Pirituba (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 61, true, true, "República", "REPÚBLICA", "Rua Exemplo, 527", true, "Segunda a sexta, 7h às 19h", "UBS República (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 62, true, false, "República", "REPÚBLICA", "Rua Modelo, 360", false, "Abre quando há aviso de risco", "Centro comunitário da República (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 63, true, true, "Santo Amaro", "SANTO AMARO", "Rua Exemplo, 541", true, "Aberto 24 horas", "UBS Santo Amaro (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 64, false, true, "Santo Amaro", "SANTO AMARO", "Rua Modelo, 370", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Santo Amaro (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 65, true, true, "São Mateus", "SÃO MATEUS", "Rua Exemplo, 555", true, "Segunda a sexta, 7h às 19h", "UBS São Mateus (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 66, false, true, "São Mateus", "SÃO MATEUS", "Rua Modelo, 380", true, "Todos os dias, 7h às 23h", "Farmácia parceira de São Mateus (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 67, true, true, "São Miguel Paulista", "SÃO MIGUEL PAULISTA", "Rua Exemplo, 569", true, "Segunda a sexta, 7h às 19h", "UBS São Miguel Paulista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 68, true, false, "São Miguel Paulista", "SÃO MIGUEL PAULISTA", "Rua Modelo, 390", false, "Abre quando há aviso de risco", "Centro comunitário de São Miguel Paulista (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 69, true, true, "Saúde", "SAÚDE", "Rua Exemplo, 583", true, "Segunda a sexta, 7h às 19h", "UBS Saúde (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 70, false, true, "Saúde", "SAÚDE", "Rua Modelo, 400", false, "Segunda a sábado, 9h às 18h", "Biblioteca da Saúde (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 71, true, true, "Sé", "SÉ", "Rua Exemplo, 597", true, "Aberto 24 horas", "UBS Sé (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 4, 4 },
                    { 72, false, true, "Sé", "SÉ", "Rua Modelo, 410", true, "Todos os dias, 7h às 23h", "Farmácia parceira da Sé (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 },
                    { 73, true, true, "Tucuruvi", "TUCURUVI", "Rua Exemplo, 611", true, "Segunda a sexta, 7h às 19h", "UBS Tucuruvi (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 5, 5 },
                    { 74, true, false, "Tucuruvi", "TUCURUVI", "Rua Modelo, 420", false, "Abre quando há aviso de risco", "Centro comunitário do Tucuruvi (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comunidade", 8, 8 },
                    { 75, true, true, "Vila Mariana", "VILA MARIANA", "Rua Exemplo, 625", true, "Segunda a sexta, 7h às 19h", "UBS Vila Mariana (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 6, 6 },
                    { 76, false, true, "Vila Mariana", "VILA MARIANA", "Rua Modelo, 430", false, "Segunda a sábado, 9h às 18h", "Biblioteca de Vila Mariana (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), false, "Biblioteca", 10, 10 },
                    { 77, true, true, "Vila Prudente", "VILA PRUDENTE", "Rua Exemplo, 639", true, "Segunda a sexta, 7h às 19h", "UBS Vila Prudente (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Saude", 7, 7 },
                    { 78, false, true, "Vila Prudente", "VILA PRUDENTE", "Rua Modelo, 440", true, "Todos os dias, 7h às 23h", "Farmácia parceira de Vila Prudente (simulado)", "Funcionando", new DateTime(2026, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, "Comercio", 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "PontosEnergia",
                keyColumn: "Id",
                keyValue: 78);
        }
    }
}
