using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkCar.Ui.Api.Migrations
{
    public partial class modelandmakes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehiclesMakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclesMakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehiclesModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleMakeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclesModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclesModels_VehiclesMakes_VehicleMakeId",
                        column: x => x.VehicleMakeId,
                        principalTable: "VehiclesMakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BestModels_VehiclesModels_VehicleModelId",
                        column: x => x.VehicleModelId,
                        principalTable: "VehiclesModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EngineModels",
                columns: table => new
                {
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsertedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Valves = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorsePowerHp = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorsePowerRpm = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TorqueFtLbs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TorqueRpm = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineModels", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_EngineModels_VehiclesModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehiclesModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelBodies",
                columns: table => new
                {
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsertedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Doors = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    EngineSize = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelBodies", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_ModelBodies_VehiclesModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehiclesModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BestModels_VehicleModelId",
                table: "BestModels",
                column: "VehicleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModels_ExternalId",
                table: "EngineModels",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelBodies_ExternalId",
                table: "ModelBodies",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclesMakes_NormalizedName",
                table: "VehiclesMakes",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclesModels_NormalizedName",
                table: "VehiclesModels",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclesModels_VehicleMakeId",
                table: "VehiclesModels",
                column: "VehicleMakeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestModels");

            migrationBuilder.DropTable(
                name: "EngineModels");

            migrationBuilder.DropTable(
                name: "ModelBodies");

            migrationBuilder.DropTable(
                name: "VehiclesModels");

            migrationBuilder.DropTable(
                name: "VehiclesMakes");
        }
    }
}
