using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class RemoveVoteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentVote",
                schema: "Teki",
                table: "Article");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentVote",
                schema: "Teki",
                table: "Article",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
