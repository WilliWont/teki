using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class AddLastUpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                schema: "Teki",
                table: "Article",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                schema: "Teki",
                table: "Article");
        }
    }
}
