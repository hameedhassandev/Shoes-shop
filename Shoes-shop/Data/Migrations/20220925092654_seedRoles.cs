using Microsoft.EntityFrameworkCore.Migrations;
using Shoes_shop.Helpers;

#nullable disable

namespace Shoes_shop.Data.Migrations
{
    public partial class seedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), RolesName.AdminRole, RolesName.AdminRole.ToUpper(), Guid.NewGuid().ToString() }
          );
            migrationBuilder.InsertData(
              table: "AspNetRoles",
             columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
             values: new object[] { Guid.NewGuid().ToString(), RolesName.UserRole, RolesName.UserRole.ToUpper(), Guid.NewGuid().ToString() }
       );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles]");

        }
    }
}
