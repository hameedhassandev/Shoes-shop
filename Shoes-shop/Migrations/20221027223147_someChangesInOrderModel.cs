using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Migrations
{
    public partial class someChangesInOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShippedAndPay",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "IsShippedAndPay",
                table: "orders");
        }
    }
}
