using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class ReAddImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                schema: "Teki",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ThumbnailImage",
                schema: "Teki",
                table: "Article",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
