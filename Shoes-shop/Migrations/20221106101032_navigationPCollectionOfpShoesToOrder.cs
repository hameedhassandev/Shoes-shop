using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Migrations
{
    public partial class navigationPCollectionOfpShoesToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_Shoes_shoesId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_shoesId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shoesId",
                table: "orders");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "shoesId",
                table: "orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_shoesId",
                table: "orders",
                column: "shoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_Shoes_shoesId",
                table: "orders",
                column: "shoesId",
                principalTable: "Shoes",
                principalColumn: "Id");
        }
    }
}
