using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderSKU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "Mixers");

            migrationBuilder.AddColumn<double>(
                name: "CleaningTime",
                table: "WIPTankLines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CleaningTimeUnit",
                table: "WIPTankLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FormatChangeTime",
                table: "SKUs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FormatChangeTimeUnit",
                table: "SKUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Capacity",
                table: "MixerBackbones",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CapacityUnit",
                table: "MixerBackbones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CleaningTime",
                table: "BIGWIPTanks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CleaningTimeUnit",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleaningTime",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "CleaningTimeUnit",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "FormatChangeTime",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "FormatChangeTimeUnit",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "MixerBackbones");

            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "MixerBackbones");

            migrationBuilder.DropColumn(
                name: "CleaningTime",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "CleaningTimeUnit",
                table: "BIGWIPTanks");

            migrationBuilder.AddColumn<double>(
                name: "Capacity",
                table: "Mixers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CapacityUnit",
                table: "Mixers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
