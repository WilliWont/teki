using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class AddBookmarkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookmarks",
                schema: "Teki",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    ArticleID = table.Column<Guid>(nullable: false),
                    DatePosted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => new { x.UserID, x.ArticleID });
                    table.ForeignKey(
                        name: "FK_Bookmarks_Article_ArticleID",
                        column: x => x.ArticleID,
                        principalSchema: "Teki",
                        principalTable: "Article",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookmarks_User_UserID",
                        column: x => x.UserID,
                        principalSchema: "Teki",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_ArticleID",
                schema: "Teki",
                table: "Bookmarks",
                column: "ArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks",
                schema: "Teki");
        }
    }
}
