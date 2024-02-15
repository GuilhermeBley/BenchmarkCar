using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkCar.Ui.Api.Migrations
{
    public partial class addresulttoprocess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingResults",
                columns: table => new
                {
                    LinkedProccessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingResults", x => x.LinkedProccessId);
                    table.ForeignKey(
                        name: "FK_ProcessingResults_ProcessingQueues_LinkedProccessId",
                        column: x => x.LinkedProccessId,
                        principalTable: "ProcessingQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessingResults");
        }
    }
}
