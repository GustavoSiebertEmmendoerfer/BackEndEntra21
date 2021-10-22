using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoFinalEntra21.Migrations
{
    public partial class v010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserPhotoId",
                table: "FileData",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileData_UserPhotoId",
                table: "FileData",
                column: "UserPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileData_AspNetUsers_UserPhotoId",
                table: "FileData",
                column: "UserPhotoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileData_AspNetUsers_UserPhotoId",
                table: "FileData");

            migrationBuilder.DropIndex(
                name: "IX_FileData_UserPhotoId",
                table: "FileData");

            migrationBuilder.DropColumn(
                name: "UserPhotoId",
                table: "FileData");
        }
    }
}
