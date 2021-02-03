using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddDocumentFieldsToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "Profile",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverLetterOriginalName",
                table: "Profile",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "Profile",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeOriginalName",
                table: "Profile",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CoverLetterOriginalName",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ResumeOriginalName",
                table: "Profile");
        }
    }
}
