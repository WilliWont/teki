using Microsoft.EntityFrameworkCore.Migrations;

namespace DataObjects.Migrations
{
    public partial class TagStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Teki",
                table: "Tag",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Teki",
                table: "Tag");
        }
    }
}
