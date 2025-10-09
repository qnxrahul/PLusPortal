using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Added_DiversityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "diversity");

            migrationBuilder.CreateTable(
                name: "AppDiversityDatas",
                schema: "diversity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HappyToCompleteForm = table.Column<int>(type: "int", nullable: true),
                    AgeRange = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    OtherGender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GenderIdentitySameAsBirth = table.Column<int>(type: "int", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    OtherSex = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SexualOrientation = table.Column<int>(type: "int", nullable: true),
                    OtherSexualOrientation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ethnicity = table.Column<int>(type: "int", nullable: true),
                    OtherEthnicity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReligionOrBelief = table.Column<int>(type: "int", nullable: true),
                    OtherReligionOrBelief = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Disability = table.Column<int>(type: "int", nullable: true),
                    EducationLevel = table.Column<int>(type: "int", nullable: true),
                    TypeOfSecondarySchool = table.Column<int>(type: "int", nullable: true),
                    OtherTypeOfSecondarySchool = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HigherEducationQualifications = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDiversityDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDiversityDatas_AppJobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "AppJobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDiversityDatas_JobApplicationId",
                schema: "diversity",
                table: "AppDiversityDatas",
                column: "JobApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDiversityDatas",
                schema: "diversity");
        }
    }
}
