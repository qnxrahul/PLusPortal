using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusFieldsIntoJobApplicationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApproveEmailStatus",
                table: "AppJobApplications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveEmailStatusUpdate",
                table: "AppJobApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejectEmailStatus",
                table: "AppJobApplications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectEmailStatusUpdate",
                table: "AppJobApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppJobApplications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdate",
                table: "AppJobApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncStatus",
                table: "AppJobApplications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncStatusUpdate",
                table: "AppJobApplications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveEmailStatus",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "ApproveEmailStatusUpdate",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "RejectEmailStatus",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "RejectEmailStatusUpdate",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "StatusUpdate",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "SyncStatus",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "SyncStatusUpdate",
                table: "AppJobApplications");
        }
    }
}
