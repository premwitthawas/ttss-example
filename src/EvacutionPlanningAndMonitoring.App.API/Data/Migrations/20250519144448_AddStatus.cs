using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvacutionPlanningAndMonitoring.App.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "EvacutionStatuses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Operations",
                table: "EvacutionStatuses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "EvacutionStatuses");

            migrationBuilder.DropColumn(
                name: "Operations",
                table: "EvacutionStatuses");
        }
    }
}
