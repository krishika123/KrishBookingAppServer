using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminIdentityServer.Data.Users.ApplicationDb
{
    public partial class profileAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "AspNetUsers");
        }
    }
}
