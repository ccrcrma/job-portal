using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace job_portal.Migrations
{
    public partial class AddCompanyJobRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "job",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("08d8c80c-8ae8-4db6-8b44-08056b25c4b0"));

            migrationBuilder.CreateIndex(
                name: "IX_job_CompanyId",
                table: "job",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_job_company_CompanyId",
                table: "job",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_company_CompanyId",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_CompanyId",
                table: "job");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "job");
        }
    }
}
