using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameFL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxCapacityGrams",
                table: "WIPTankLines",
                newName: "CapacityKgr");

            migrationBuilder.RenameColumn(
                name: "WeightPerUnit",
                table: "SKUs",
                newName: "mLPerUnit");

            migrationBuilder.RenameColumn(
                name: "PlannedWeightGrams",
                table: "ProductionScheduleItems",
                newName: "PlannedWeightKgr");

            migrationBuilder.RenameColumn(
                name: "MaxCapacityGrams",
                table: "Mixers",
                newName: "CapacityKg");

            migrationBuilder.RenameColumn(
                name: "BatchCycleTime",
                table: "MixerBackbones",
                newName: "BatchCycleTimeMin");

            migrationBuilder.RenameColumn(
                name: "MaxUnitsPerHour",
                table: "LineSpeeds",
                newName: "UnitsPerMin");

            migrationBuilder.RenameColumn(
                name: "MaxCapacityGrams",
                table: "BIGWIPTanks",
                newName: "CapacityKgr");

            migrationBuilder.AddColumn<double>(
                name: "grPerUnit",
                table: "SKUs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "grPerUnit",
                table: "SKUs");

            migrationBuilder.RenameColumn(
                name: "CapacityKgr",
                table: "WIPTankLines",
                newName: "MaxCapacityGrams");

            migrationBuilder.RenameColumn(
                name: "mLPerUnit",
                table: "SKUs",
                newName: "WeightPerUnit");

            migrationBuilder.RenameColumn(
                name: "PlannedWeightKgr",
                table: "ProductionScheduleItems",
                newName: "PlannedWeightGrams");

            migrationBuilder.RenameColumn(
                name: "CapacityKg",
                table: "Mixers",
                newName: "MaxCapacityGrams");

            migrationBuilder.RenameColumn(
                name: "BatchCycleTimeMin",
                table: "MixerBackbones",
                newName: "BatchCycleTime");

            migrationBuilder.RenameColumn(
                name: "UnitsPerMin",
                table: "LineSpeeds",
                newName: "MaxUnitsPerHour");

            migrationBuilder.RenameColumn(
                name: "CapacityKgr",
                table: "BIGWIPTanks",
                newName: "MaxCapacityGrams");
        }
    }
}
