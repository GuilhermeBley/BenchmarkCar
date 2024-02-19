using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkCar.Ui.Api.Migrations
{
    public partial class addenginesizeproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MetaData",
                table: "ProcessingQueues",
                type: "VARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)");

            migrationBuilder.AddColumn<decimal>(
                name: "EngineSize",
                table: "EngineModels",
                type: "decimal(10,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineSize",
                table: "EngineModels");

            migrationBuilder.AlterColumn<string>(
                name: "MetaData",
                table: "ProcessingQueues",
                type: "VARCHAR(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
