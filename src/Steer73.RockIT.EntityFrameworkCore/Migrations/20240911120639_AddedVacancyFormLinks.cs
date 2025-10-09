using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class AddedVacancyFormLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiversityFormDefinitionId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VacancyFormDefinitionId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancies_DiversityFormDefinitionId",
                table: "AppVacancies",
                column: "DiversityFormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancies_VacancyFormDefinitionId",
                table: "AppVacancies",
                column: "VacancyFormDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppVacancies_AppFormDefinitions_DiversityFormDefinitionId",
                table: "AppVacancies",
                column: "DiversityFormDefinitionId",
                principalTable: "AppFormDefinitions", 
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppVacancies_AppFormDefinitions_VacancyFormDefinitionId",
                table: "AppVacancies",
                column: "VacancyFormDefinitionId",
                principalTable: "AppFormDefinitions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVacancies_AppFormDefinitions_DiversityFormDefinitionId",
                table: "AppVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_AppVacancies_AppFormDefinitions_VacancyFormDefinitionId",
                table: "AppVacancies");

            migrationBuilder.DropIndex(
                name: "IX_AppVacancies_DiversityFormDefinitionId",
                table: "AppVacancies");

            migrationBuilder.DropIndex(
                name: "IX_AppVacancies_VacancyFormDefinitionId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "DiversityFormDefinitionId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "VacancyFormDefinitionId",
                table: "AppVacancies");
        }
    }
}
