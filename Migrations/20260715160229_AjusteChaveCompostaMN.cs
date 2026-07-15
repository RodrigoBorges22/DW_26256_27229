using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DW_26256_27229.Migrations
{
    /// <inheritdoc />
    public partial class AjusteChaveCompostaMN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscricoes",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_EventoId",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Inscricoes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscricoes",
                table: "Inscricoes",
                columns: new[] { "EventoId", "UtilizadorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscricoes",
                table: "Inscricoes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Inscricoes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscricoes",
                table: "Inscricoes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_EventoId",
                table: "Inscricoes",
                column: "EventoId");
        }
    }
}
