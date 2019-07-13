using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class ii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "StartYear",
                table: "WorkEx",
                newName: "Technologies_Used");

            migrationBuilder.AddColumn<int>(
                name: "No_Of_Years",
                table: "WorkEx",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No_Of_Years",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "Technologies_Used",
                table: "WorkEx",
                newName: "StartYear");

            migrationBuilder.AddColumn<string>(
                name: "EndYear",
                table: "WorkEx",
                nullable: true);
        }
    }
}
