using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Added_CompanyRole_User_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyRole",
                table: "AbpUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyRole",
                table: "AbpUsers");
        }
    }
}
