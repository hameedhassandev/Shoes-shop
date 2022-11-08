using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Migrations
{
    public partial class navigationPtopShoesToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
