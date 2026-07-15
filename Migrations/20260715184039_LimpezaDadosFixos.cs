using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DW_26256_27229.Migrations
{
    /// <inheritdoc />
    public partial class LimpezaDadosFixos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Workshop Técnico" },
                    { 2, "Palestra" },
                    { 3, "Torneio de Gaming" }
                });

            migrationBuilder.InsertData(
                table: "Utilizadores",
                columns: new[] { "Id", "Email", "Nome", "Password", "TipoUtilizador" },
                values: new object[,]
                {
                    { 1, "admin@ipt.pt", "Administrador Principal", "123", "Admin" },
                    { 2, "aluno@ipt.pt", "Aluno Teste", "123", "Aluno" },
                    { 3, "professor@ipt.pt", "Professor Teste", "123", "Professor" }
                });

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "Id", "CategoriaId", "DataHora", "Titulo", "UtilizadorId", "VagasMaximas" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), "Introdução a Redes Cisco e Routing", 1, 20 },
                    { 2, 3, new DateTime(2026, 11, 10, 18, 30, 0, 0, DateTimeKind.Unspecified), "Torneio Local Co-op (Rayman Legends)", 1, 16 }
                });
        }
    }
}
