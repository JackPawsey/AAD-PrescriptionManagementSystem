using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class GeneralPractioner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralPractioner",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSNumber",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralPractioner",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NHSNumber",
                table: "AspNetUsers");
        }
    }
}
