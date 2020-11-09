using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class RemoveImageMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "Teki",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ThumbnailImage",
                schema: "Teki",
                table: "Article");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                schema: "Teki",
                table: "Article",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ThumbnailImage",
                schema: "Teki",
                table: "Article",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
