using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DW_26256_27229.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCriadorEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "Eventos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UtilizadorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UtilizadorId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UtilizadorId",
                table: "Eventos",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Utilizadores_UtilizadorId",
                table: "Eventos",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Utilizadores_UtilizadorId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_UtilizadorId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Eventos");
        }
    }
}
