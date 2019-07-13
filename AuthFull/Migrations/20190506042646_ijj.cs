using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab11Authorization.Migrations
{
    public partial class ijj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkEx_Applicant_ApplicantId",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "WorkEx",
                newName: "applicantNApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkEx_ApplicantId",
                table: "WorkEx",
                newName: "IX_WorkEx_applicantNApplicantId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantIds",
                table: "WorkEx",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkEx_Applicant_applicantNApplicantId",
                table: "WorkEx",
                column: "applicantNApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkEx_Applicant_applicantNApplicantId",
                table: "WorkEx");

            migrationBuilder.DropColumn(
                name: "ApplicantIds",
                table: "WorkEx");

            migrationBuilder.RenameColumn(
                name: "applicantNApplicantId",
                table: "WorkEx",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkEx_applicantNApplicantId",
                table: "WorkEx",
                newName: "IX_WorkEx_ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkEx_Applicant_ApplicantId",
                table: "WorkEx",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
