using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class Iniy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number_Of_Years",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "Description_Of_Work",
                table: "WorkEx",
                newName: "Company");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndYear",
                table: "WorkEx",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "WorkEx",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsibilites",
                table: "WorkEx",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartYear",
                table: "WorkEx",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "WorkEx");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "WorkEx");

            migrationBuilder.DropColumn(
                name: "Responsibilites",
                table: "WorkEx");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "WorkEx",
                newName: "Description_Of_Work");

            migrationBuilder.AddColumn<int>(
                name: "Number_Of_Years",
                table: "WorkEx",
                nullable: false,
                defaultValue: 0);
        }
    }
}
