using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameFL3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsPerMin",
                table: "LineSpeeds");

            migrationBuilder.RenameColumn(
                name: "CapacityKgr",
                table: "WIPTankLines",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "mLPerUnit",
                table: "SKUs",
                newName: "VolumePerEA");

            migrationBuilder.RenameColumn(
                name: "grPerUnit",
                table: "SKUs",
                newName: "MassPerEA");

            migrationBuilder.RenameColumn(
                name: "PlannedWeightKgr",
                table: "ProductionScheduleItems",
                newName: "PlannedMass");

            migrationBuilder.RenameColumn(
                name: "CapacityKg",
                table: "Mixers",
                newName: "CleaningTime");

            migrationBuilder.RenameColumn(
                name: "BatchCycleTimeMin",
                table: "MixerBackbones",
                newName: "BatchTime");

            migrationBuilder.RenameColumn(
                name: "CapacityKgr",
                table: "BIGWIPTanks",
                newName: "Capacity");

            migrationBuilder.AddColumn<string>(
                name: "CapacityUnit",
                table: "WIPTankLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WIPTankLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MassPerEAUnit",
                table: "SKUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VolumePerEAUnit",
                table: "SKUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlannedMassUnit",
                table: "ProductionScheduleItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ProductionPlans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ProductionPlans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductionPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductionLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "CleaningTimeUnit",
                table: "Mixers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BatchTimeUnit",
                table: "MixerBackbones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeed",
                table: "LineSpeeds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MaxSpeedUnit",
                table: "LineSpeeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CapacityUnit",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "MassPerEAUnit",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "VolumePerEAUnit",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "PlannedMassUnit",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductionPlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "CleaningTimeUnit",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "BatchTimeUnit",
                table: "MixerBackbones");

            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                table: "LineSpeeds");

            migrationBuilder.DropColumn(
                name: "MaxSpeedUnit",
                table: "LineSpeeds");

            migrationBuilder.DropColumn(
                name: "CapacityUnit",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BIGWIPTanks");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "WIPTankLines",
                newName: "CapacityKgr");

            migrationBuilder.RenameColumn(
                name: "VolumePerEA",
                table: "SKUs",
                newName: "mLPerUnit");

            migrationBuilder.RenameColumn(
                name: "MassPerEA",
                table: "SKUs",
                newName: "grPerUnit");

            migrationBuilder.RenameColumn(
                name: "PlannedMass",
                table: "ProductionScheduleItems",
                newName: "PlannedWeightKgr");

            migrationBuilder.RenameColumn(
                name: "CleaningTime",
                table: "Mixers",
                newName: "CapacityKg");

            migrationBuilder.RenameColumn(
                name: "BatchTime",
                table: "MixerBackbones",
                newName: "BatchCycleTimeMin");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "BIGWIPTanks",
                newName: "CapacityKgr");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ProductionPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ProductionPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitsPerMin",
                table: "LineSpeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
