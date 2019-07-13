using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class Inu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Race",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applicant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Applicant",
                nullable: true);
        }
    }
}
