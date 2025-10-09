using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Practice_Group_Column_On_Vacancies_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVacancies_AppPracticeGroups_PracticeGroupId",
                table: "AppVacancies");

            migrationBuilder.DropIndex(
                name: "IX_AppVacancies_PracticeGroupId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "PracticeGroupId",
                table: "AppVacancies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PracticeGroupId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancies_PracticeGroupId",
                table: "AppVacancies",
                column: "PracticeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppVacancies_AppPracticeGroups_PracticeGroupId",
                table: "AppVacancies",
                column: "PracticeGroupId",
                principalTable: "AppPracticeGroups",
                principalColumn: "Id");
        }
    }
}
