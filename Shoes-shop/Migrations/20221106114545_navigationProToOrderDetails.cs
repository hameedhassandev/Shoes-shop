using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Migrations
{
    public partial class navigationProToOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_orders_OrderId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_OrderId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Shoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Shoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_OrderId",
                table: "Shoes",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_orders_OrderId",
                table: "Shoes",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id");
        }
    }
}
