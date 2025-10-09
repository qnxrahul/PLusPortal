using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class JobApplicant_Files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDocumentId",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "CVId",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "CoverLetterId",
                table: "AppJobApplications");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalDocumentUrl",
                table: "AppJobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CVUrl",
                table: "AppJobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverLetterUrl",
                table: "AppJobApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDocumentUrl",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "CVUrl",
                table: "AppJobApplications");

            migrationBuilder.DropColumn(
                name: "CoverLetterUrl",
                table: "AppJobApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "AdditionalDocumentId",
                table: "AppJobApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CVId",
                table: "AppJobApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoverLetterId",
                table: "AppJobApplications",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
