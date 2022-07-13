using Microsoft.EntityFrameworkCore.Migrations;

namespace EBookkeepingAuth.Data.Users.ApplicationDb
{
    public partial class addedbusinessRegistrationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessRegistrationNumber",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessRegistrationNumber",
                table: "AspNetUsers");
        }
    }
}
