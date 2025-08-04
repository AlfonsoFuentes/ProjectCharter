using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderProductionplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinimumLevelPercentage",
                table: "WIPTankLines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
                name: "CleaningTime",
                table: "ProductionLines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CleaningTimeUnit",
                table: "ProductionLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "InletMassFlow",
                table: "BIGWIPTanks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "InletMassFlowUnit",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MinimumTransferLevelKgPercentage",
                table: "BIGWIPTanks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OutletMassFlow",
                table: "BIGWIPTanks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OutletMassFlowUnit",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumLevelPercentage",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "FormatChangeTime",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "FormatChangeTimeUnit",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "CleaningTime",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "CleaningTimeUnit",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "InletMassFlow",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "InletMassFlowUnit",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "MinimumTransferLevelKgPercentage",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "OutletMassFlow",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "OutletMassFlowUnit",
                table: "BIGWIPTanks");
        }
    }
}
