using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DW_26256_27229.Migrations
{
    /// <inheritdoc />
    public partial class InjetaProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Utilizadores",
                columns: new[] { "Id", "Email", "Nome", "Password", "TipoUtilizador" },
                values: new object[] { 3, "professor@ipt.pt", "Professor Teste", "123", "Professor" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
