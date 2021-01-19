using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddColumnsToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "job",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "job",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Vacancy",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "job");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "job");

            migrationBuilder.DropColumn(
                name: "Vacancy",
                table: "job");
        }
    }
}
