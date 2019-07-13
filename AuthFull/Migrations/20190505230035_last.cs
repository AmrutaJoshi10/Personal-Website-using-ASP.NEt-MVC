using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class last : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "projects",
                table: "Academics",
                newName: "project_Links");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Academics",
                newName: "Description_About_Projects");

            migrationBuilder.AlterColumn<string>(
                name: "StartYear",
                table: "WorkEx",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "EndYear",
                table: "WorkEx",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GPA",
                table: "Academics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Academics");

            migrationBuilder.RenameColumn(
                name: "project_Links",
                table: "Academics",
                newName: "projects");

            migrationBuilder.RenameColumn(
                name: "Description_About_Projects",
                table: "Academics",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartYear",
                table: "WorkEx",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndYear",
                table: "WorkEx",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
