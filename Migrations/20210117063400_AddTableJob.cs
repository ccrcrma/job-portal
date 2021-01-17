using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddTableJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Negotiable"),
                    Type = table.Column<sbyte>(type: "tinyint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job");
        }
    }
}
