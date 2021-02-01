using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddFieldsToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Profile",
                newName: "Experience");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Profile",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Profile");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Profile",
                newName: "Description");
        }
    }
}
