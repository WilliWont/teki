using Microsoft.EntityFrameworkCore.Migrations;

namespace TekiBlog.Migrations
{
    public partial class ArticleFieldRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tittle",
                schema: "Tekki",
                table: "Article",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Sumary",
                schema: "Tekki",
                table: "Article",
                newName: "Summary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Status_StatusID",
                schema: "Tekki",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Article_User_UserId",
                schema: "Tekki",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Summary",
                schema: "Tekki",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Tekki",
                table: "Article");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Tekki",
                table: "Article",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                schema: "Tekki",
                table: "Article",
                newName: "statusID");

            migrationBuilder.RenameIndex(
                name: "IX_Article_UserId",
                schema: "Tekki",
                table: "Article",
                newName: "IX_Article_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Article_StatusID",
                schema: "Tekki",
                table: "Article",
                newName: "IX_Article_statusID");

            migrationBuilder.AddColumn<string>(
                name: "Sumary",
                schema: "Tekki",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tittle",
                schema: "Tekki",
                table: "Article",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Status_statusID",
                schema: "Tekki",
                table: "Article",
                column: "statusID",
                principalSchema: "Tekki",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Article_User_userId",
                schema: "Tekki",
                table: "Article",
                column: "userId",
                principalSchema: "Tekki",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
