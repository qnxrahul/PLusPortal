using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaSourceAndRoleTypeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE AppVacancies
SET Region = '1'
");

            migrationBuilder.DropColumn(
                name: "MediaSources",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "AppVacancies");

            migrationBuilder.AlterColumn<int>(
                name: "Region",
                table: "AppVacancies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateTable(
                name: "AppMediaSources",
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
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMediaSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleTypes",
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
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppVacancyMediaSources",
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
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaSourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVacancyMediaSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVacancyMediaSources_AppMediaSources_MediaSourceId",
                        column: x => x.MediaSourceId,
                        principalTable: "AppMediaSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppVacancyMediaSources_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppVacancyRoleTypes",
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
                    VacancyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVacancyRoleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVacancyRoleTypes_AppRoleTypes_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalTable: "AppRoleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppVacancyRoleTypes_AppVacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "AppVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyMediaSources_MediaSourceId",
                table: "AppVacancyMediaSources",
                column: "MediaSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyMediaSources_VacancyId",
                table: "AppVacancyMediaSources",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyRoleTypes_RoleTypeId",
                table: "AppVacancyRoleTypes",
                column: "RoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVacancyRoleTypes_VacancyId",
                table: "AppVacancyRoleTypes",
                column: "VacancyId");

            migrationBuilder.Sql(@"

INSERT INTO AppMediaSources
	(Id, ExtraProperties, ConcurrencyStamp, CreationTime, CreatorId, Name, Description)
VALUES
('5c6dd2c7-715d-4bb8-a7a4-6ba02ec09efe', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_The Guardian', 'Website_The Guardian')
,('52b5a10c-fee3-46d5-8efa-f4fb8528e4a4', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_The Economist', 'Website_The Economist')
,('a59ef2b0-5eae-45d5-a06b-d199176e9684', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Sunday Times', 'Website_Sunday Times')
,('b1860c71-4374-4481-8ca0-0c0ed11f3566', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Exec Appointments', 'Website_Exec Appointments')
,('0f7a3874-1cd4-471e-b248-3d970765d65d', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Jobs.ac.uk', 'Website_Jobs.ac.uk')
,('ee14520a-0493-494a-b0ed-e89d10117754', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_TES', 'Website_TES')
,('2d1cb99a-6a1e-4db5-8037-0c2ddcba0f18', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Times Higher Education (THE)', 'Website_Times Higher Education (THE)')
,('d8101274-baab-49fb-8550-0f857b099fd1', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Unijobs', 'Website_Unijobs')
,('549cb3be-7c09-4f04-a567-47d4ca8b254d', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Diversity Jobsite', 'Website_Diversity Jobsite')
,('f5820efe-9241-4da3-9c42-704a2b9ddf69', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Divesifying', 'Website_Divesifying')
,('dad4c520-8183-44fb-9147-86b1ae3b39c2', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Evenbreak', 'Website_Evenbreak')
,('730fbbd3-a0cd-457e-8343-4388a1f79df2', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Board Appointments', 'Website_Board Appointments')
,('604d5ffb-7c4d-4b49-a063-c44e906adff7', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_NED on Board', 'Website_NED on Board')
,('965ba5e3-51c4-4ee1-bd2e-57d3ff34d5df', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_University Chairs', 'Website_University Chairs')
,('160e4695-efa4-4e13-8e03-31fe9d7341ad', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Women on Boards', 'Website_Women on Boards')
,('0b2e6f39-9f82-4523-a285-012126465de4', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_US Chronicle of Education', 'Website_US Chronicle of Education')
,('de65b22c-9309-41fe-b28a-4273d51eecc5', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_University Affairs', 'Website_University Affairs')
,('be08c460-b237-41c4-965d-49ea4a56c7d0', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Devex', 'Website_Devex')
,('0db273a9-2acd-40a8-a798-52b43f2aa6e1', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Devnet Jobs', 'Website_Devnet Jobs')
,('0665194c-98c7-4fe1-a9ec-1b4766ce48fe', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Idealist', 'Website_Idealist')
,('7cd7c4ba-f039-4041-b3a2-0fed24050376', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Philanthropy.com', 'Website_Philanthropy.com')
,('94c3f81f-a5bb-4533-9ec4-258972014917', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Global Charity Jobs', 'Website_Global Charity Jobs')
,('337f57d5-dc31-4168-938b-0d3f46650c08', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_De Volkskrant', 'Website_De Volkskrant')
,('1ffe9085-9ade-4a9a-b6e5-567fa3dca57c', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Euro Brussels', 'Website_Euro Brussels')
,('8dc87a0d-b93e-434a-b780-b86b36700a41', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Nrccarriere.nl', 'Website_Nrccarriere.nl')
,('7afdaf64-4d6a-4c66-8e5f-da2ed75017d1', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_ Nature', 'Website_ Nature')
,('3a3cf38a-ce41-49ed-8e22-0d83d9fd21bf', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Science Mag', 'Website_Science Mag')
,('19cf7044-4af7-4aad-8a5e-5c6411496d26', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Environment Career', 'Website_Environment Career')
,('53ff0513-3318-4d68-a709-87ef98377272', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Environment Jobs', 'Website_Environment Jobs')
,('1f8397da-1328-4209-a626-61722ebafe6e', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Artsjobs.org.uk', 'Website_Artsjobs.org.uk')
,('87551b5d-2b65-48d2-8024-e0db91ea89fe', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Arts Professional', 'Website_Arts Professional')
,('dfe62f55-1728-4dd7-b959-d3bbe644fca9', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Sporting Equals', 'Website_Sporting Equals')
,('1f1ed44f-4678-4a64-89a0-91b442a71003', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_UK Sport', 'Website_UK Sport')
,('b66e6996-d9cd-49c5-b1c3-5e763bf0543c', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Perrett Laver', 'Website_Perrett Laver')
,('016fac28-859e-43a9-959f-cd04fcdd34a5', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Organisations own website', 'Website_Organisations own website')
,('bb634ed3-2c14-46a2-8277-bd41ee95e0d1', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_Google For Jobs', 'Website_Google For Jobs')
,('8d71029b-5daa-4eb0-a930-335c3c585fab', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Website_google/bing/Yahoo etc)', 'Website_google/bing/Yahoo etc)')
,('6ccf7e8f-9836-49e0-aeda-aa6635d2c812', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Social Media_ LinkedIn', 'Social Media_ LinkedIn')
,('dfbbf037-23fe-4346-b36c-ef99cae11a98', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Social Media_ X', 'Social Media_ X')
,('42b9f044-8f14-42ff-a976-865cc9d94797', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Job board_Indeed', 'Job board_Indeed')
,('99beb018-ca2e-4205-933f-6352145b23b9', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Jobboard_JobServe', 'Jobboard_JobServe')
,('aaa69a73-11e7-48c6-a7fd-30ef4d78e9f5', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Governance Apprenticeship Programme', 'Governance Apprenticeship Programme')
,('758e1b8f-2ad2-4229-ad33-9ed1615b1c92', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Print_Newspaper', 'Print_Newspaper')
,('d06acc63-944d-460c-8e58-1afdeb54b209', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Print_Journal', 'Print_Journal')
,('79eb4fdd-0bdb-4ead-898c-9bc9507839a2', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'The Chronicle of Philanthropy', 'The Chronicle of Philanthropy')
,('1279aceb-5a94-4ade-8455-8f1969d2346e', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'THE unijobs', 'THE unijobs')
,('e7410060-507c-4d22-8438-78a5b194121f', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'The Lancet', 'The Lancet')
,('1dd57af4-5766-4dc9-96e2-219590503f2b', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Euraxess', 'Euraxess')
,('42cf7731-eebe-40a3-83ab-286543f45c58', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'De Zeit', 'De Zeit')
,('fe70c6cc-29c8-4e84-91ef-6382c6999f0b', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'The Irish Times', 'The Irish Times')
,('46b0cb5e-5bbb-4eac-944b-c1a32c45e9e8', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Academic Transfer', 'Academic Transfer')
,('3f4cf8b0-8811-4532-8dbd-3b74abc46392', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Academic Positions', 'Academic Positions')

INSERT INTO AppRoleTypes
	(Id, ExtraProperties, ConcurrencyStamp, CreationTime, CreatorId, Name, Description)
VALUES
('0df35f61-0644-4ff1-ab9b-b99341a0db47', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Remote', 'Remote')
,('3ce54dbc-ab2d-4177-be96-f43e1f725d2c', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Office', 'Office')
,('7cb59348-021f-43d6-9325-465516fd726d', '{}', '5a4045d6591b434d84e2add0bafa4496', GETDATE(), NULL, 'Hybrid', 'Hybrid')

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVacancyMediaSources");

            migrationBuilder.DropTable(
                name: "AppVacancyRoleTypes");

            migrationBuilder.DropTable(
                name: "AppMediaSources");

            migrationBuilder.DropTable(
                name: "AppRoleTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "MediaSources",
                table: "AppVacancies",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleType",
                table: "AppVacancies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
