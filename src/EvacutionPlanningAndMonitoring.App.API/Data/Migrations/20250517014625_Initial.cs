using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvacutionPlanningAndMonitoring.App.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvacutionZones",
                columns: table => new
                {
                    ZoneID = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "integer", nullable: false),
                    UrgencyLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacutionZones", x => x.ZoneID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VehicleID = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Speed = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.VehicleID);
                });

            migrationBuilder.CreateTable(
                name: "EvacutionStatuses",
                columns: table => new
                {
                    ZoneID = table.Column<string>(type: "text", nullable: false),
                    TotalEvacuated = table.Column<int>(type: "integer", nullable: false),
                    RemainingPeople = table.Column<int>(type: "integer", nullable: false),
                    LastVechicleUsed = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacutionStatuses", x => x.ZoneID);
                    table.ForeignKey(
                        name: "FK_EvacutionStatuses_EvacutionZones_ZoneID",
                        column: x => x.ZoneID,
                        principalTable: "EvacutionZones",
                        principalColumn: "ZoneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvacutionPlans",
                columns: table => new
                {
                    ZoneID = table.Column<string>(type: "text", nullable: false),
                    VehicleID = table.Column<string>(type: "text", nullable: false),
                    ETA = table.Column<string>(type: "text", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacutionPlans", x => new { x.ZoneID, x.VehicleID });
                    table.ForeignKey(
                        name: "FK_EvacutionPlans_EvacutionZones_ZoneID",
                        column: x => x.ZoneID,
                        principalTable: "EvacutionZones",
                        principalColumn: "ZoneID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvacutionPlans_Vehicle_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvacutionPlans_VehicleID",
                table: "EvacutionPlans",
                column: "VehicleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvacutionPlans");

            migrationBuilder.DropTable(
                name: "EvacutionStatuses");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "EvacutionZones");
        }
    }
}
