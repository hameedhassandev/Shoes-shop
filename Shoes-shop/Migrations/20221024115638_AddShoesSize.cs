using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Migrations
{
    public partial class AddShoesSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Shoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Shoes");
        }
    }
}
