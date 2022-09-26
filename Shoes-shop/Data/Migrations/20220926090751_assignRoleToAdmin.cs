using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoes_shop.Data.Migrations
{
    public partial class assignRoleToAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] (UserId,RoleId) VALUES ('57e6c063-76df-40c0-80a4-94252a1af938', '44621a81-4783-40dd-abe8-cf7d351750b9')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles]");        }
    }
}
