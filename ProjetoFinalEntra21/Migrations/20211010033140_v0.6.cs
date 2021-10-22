using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoFinalEntra21.Migrations
{
    public partial class v06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseTime",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpenTime",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "AspNetUsers");
        }
    }
}
