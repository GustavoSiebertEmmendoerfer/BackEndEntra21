using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoFinalEntra21.Migrations
{
    public partial class vp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "photoPath",
                table: "Plate",
                newName: "PhotoURL");

            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "AspNetUsers",
                newName: "PhotoURL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "Plate",
                newName: "photoPath");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "AspNetUsers",
                newName: "PhotoPath");
        }
    }
}
