using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Vacancy_24083013155449 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MediaSources",
                table: "AppVacancies",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdditionalFileId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BrochureFileId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosingDate",
                table: "AppVacancies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppVacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiringDate",
                table: "AppVacancies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExternalPostingDate",
                table: "AppVacancies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "FormalInterviewDate",
                table: "AppVacancies",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PracticeGroupId",
                table: "AppVacancies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleType",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salary",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SecondInterviewDate",
                table: "AppVacancies",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppFileDescriptors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFileDescriptors", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVacancies_AppPracticeGroups_PracticeGroupId",
                table: "AppVacancies");

            migrationBuilder.DropTable(
                name: "AppFileDescriptors");

            migrationBuilder.DropIndex(
                name: "IX_AppVacancies_PracticeGroupId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "AdditionalFileId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "BrochureFileId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "ExpiringDate",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "ExternalPostingDate",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "FormalInterviewDate",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "PracticeGroupId",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "SecondInterviewDate",
                table: "AppVacancies");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MediaSources",
                table: "AppVacancies",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);
        }
    }
}
