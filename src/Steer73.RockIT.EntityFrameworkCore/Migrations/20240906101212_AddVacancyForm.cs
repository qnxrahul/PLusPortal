using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class AddVacancyForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVacancyForms",
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
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FormDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "IX_AppVacancyForms_FormDefinitionId",
                table: "AppVacancyForms",
                column: "FormDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyForms_VacancyApplicationId",
                table: "AppVacancyForms",
                column: "VacancyApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVacancyForms");
        }
    }
}
