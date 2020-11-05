using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class Tag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "Teki",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                schema: "Teki",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Teki",
                        principalTable: "Article",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Teki",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagId",
                schema: "Teki",
                table: "ArticleTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag",
                schema: "Teki");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "Teki");
        }
    }
}
