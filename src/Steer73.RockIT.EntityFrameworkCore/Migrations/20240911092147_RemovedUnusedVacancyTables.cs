using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnusedVacancyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVacancyFormDefinitions");

            migrationBuilder.DropTable(
                name: "AppVacancyForms");

            migrationBuilder.DropTable(
                name: "AppVacancyApplications");

            migrationBuilder.DropTable(
                name: "AppApplicants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppApplicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CoverLetterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentCompany = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentPositionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentRole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CvId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Landline = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "AppVacancyFormDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVacancyFormDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVacancyFormDefinitions_AppFormDefinitions_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "AppFormDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVacancyFormDefinitions_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppVacancyApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVacancyApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVacancyApplications_AppApplicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AppApplicants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVacancyApplications_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppVacancyForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VacancyApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVacancyForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVacancyForms_AppFormDefinitions_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "AppFormDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVacancyForms_AppVacancyApplications_VacancyApplicationId",
                        column: x => x.VacancyApplicationId,
                        principalTable: "AppVacancyApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppApplicants_VacancyId",
                table: "AppApplicants",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyApplications_ApplicantId",
                table: "AppVacancyApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyApplications_VacancyId",
                table: "AppVacancyApplications",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyFormDefinitions_FormDefinitionId",
                table: "AppVacancyFormDefinitions",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyFormDefinitions_VacancyId",
                table: "AppVacancyFormDefinitions",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyForms_FormDefinitionId",
                table: "AppVacancyForms",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyForms_VacancyApplicationId",
                table: "AppVacancyForms",
                column: "VacancyApplicationId");
        }
    }
}
