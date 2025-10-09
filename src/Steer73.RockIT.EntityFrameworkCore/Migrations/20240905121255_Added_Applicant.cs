using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Added_Applicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppApplicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Landline = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentRole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentCompany = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentPositionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CvId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoverLetterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdditionalDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApplicants_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppApplicants_VacancyId",
                table: "AppApplicants",
                column: "VacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppApplicants");
        }
    }
}
