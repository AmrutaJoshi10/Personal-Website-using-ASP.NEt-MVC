using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class uuu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date_Of_Birth",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone_Number",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Academics",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "projects",
                table: "Academics",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkEx",
                columns: table => new
                {
                    WorkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description_Of_Work = table.Column<string>(maxLength: 5000, nullable: true),
                    Number_Of_Years = table.Column<int>(nullable: false),
                    ApplicantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkEx", x => x.WorkId);
                    table.ForeignKey(
                        name: "FK_WorkEx_Applicant_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicant",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkEx_ApplicantId",
                table: "WorkEx",
                column: "ApplicantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkEx");

            migrationBuilder.DropColumn(
                name: "Date_Of_Birth",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Phone_Number",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Academics");

            migrationBuilder.DropColumn(
                name: "projects",
                table: "Academics");
        }
    }
}
