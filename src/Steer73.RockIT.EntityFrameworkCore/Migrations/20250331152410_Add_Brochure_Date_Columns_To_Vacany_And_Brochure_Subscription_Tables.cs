using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steer73.RockIT.Migrations
{
    /// <inheritdoc />
    public partial class Add_Brochure_Date_Columns_To_Vacany_And_Brochure_Subscription_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BrochureLastUpdatedAt",
                table: "AppVacancies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "AppBrochureSubscriptions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrochureLastUpdatedAt",
                table: "AppVacancies");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "AppBrochureSubscriptions");
        }
    }
}
