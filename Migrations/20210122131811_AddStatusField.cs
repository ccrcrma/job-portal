using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddStatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<sbyte>(
                name: "Status",
                table: "post",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)2);

            migrationBuilder.AddColumn<sbyte>(
                name: "Status",
                table: "job",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "post");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "job");
        }
    }
}
