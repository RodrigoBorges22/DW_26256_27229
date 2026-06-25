using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DW_26256_27229.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPasswordUtilizador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Utilizadores",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "123");

            migrationBuilder.UpdateData(
                table: "Utilizadores",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Utilizadores");
        }
    }
}
