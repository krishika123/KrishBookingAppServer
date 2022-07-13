using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EBookkeepingAuth.Data.Users.ApplicationDb
{
    public partial class TransactionCurrencyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransactionCurrency",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionCurrency",
                table: "AspNetUsers");
        }
    }
}
