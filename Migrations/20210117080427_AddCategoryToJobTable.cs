using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddCategoryToJobTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 17);

            migrationBuilder.CreateIndex(
                name: "IX_job_category_id",
                table: "job",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_category_category_id",
                table: "job",
                column: "category_id",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_category_category_id",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_category_id",
                table: "job");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "job");
        }
    }
}
