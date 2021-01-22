using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddColumnImageNameToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "post",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "post");
        }
    }
}
