using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Added_BrochureSubscriptions_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBrochureSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CurrentRole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CurrentCompany = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBrochureSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBrochureSubscriptions_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBrochureSubscriptions_VacancyId",
                table: "AppBrochureSubscriptions",
                column: "VacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBrochureSubscriptions");
        }
    }
}
