using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkCar.Ui.Api.Migrations
{
    public partial class metadatakindforProcessingState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaData",
                table: "ProcessingQueues",
                type: "VARCHAR(MAX)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaData",
                table: "ProcessingQueues");
        }
    }
}
