using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoFinalEntra21.Migrations
{
    public partial class v011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_RestaurantId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_RestaurantId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Order",
                newName: "RestaurantEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantEmail",
                table: "Order",
                newName: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RestaurantId",
                table: "Order",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_RestaurantId",
                table: "Order",
                column: "RestaurantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
