using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class ModifyColumnSalaryInJobTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "job");

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMax",
                table: "job",
                type: "decimal(13,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMin",
                table: "job",
                type: "decimal(13,4)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryMax",
                table: "job");

            migrationBuilder.DropColumn(
                name: "SalaryMin",
                table: "job");

            migrationBuilder.AddColumn<string>(
                name: "Salary",
                table: "job",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Negotiable");
        }
    }
}
