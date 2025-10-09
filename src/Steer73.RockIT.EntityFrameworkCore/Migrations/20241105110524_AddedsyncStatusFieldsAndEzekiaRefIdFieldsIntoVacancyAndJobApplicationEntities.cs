using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class AddedsyncStatusFieldsAndEzekiaRefIdFieldsIntoVacancyAndJobApplicationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalRefId",
                table: "AppVacancies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncStatus",
                table: "AppVacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncStatusUpdate",
                table: "AppVacancies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalRefId",
                table: "AppJobApplications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRefId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "SyncStatus",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "SyncStatusUpdate",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "ExternalRefId",
                table: "AppJobApplications");
        }
    }
}
