using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionplanWIPS2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "TankCode",
                table: "WIPTankLines");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "Presentation",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductionPlans");

            migrationBuilder.DropColumn(
                name: "PlanCode",
                table: "ProductionPlans");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "LineCode",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "DisabledDate",
                table: "MixerBackbones");

            migrationBuilder.DropColumn(
                name: "CurrentItemIndex",
                table: "LineProductionSchedules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "TankCode",
                table: "BIGWIPTanks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Backbones");

            migrationBuilder.AddColumn<double>(
                name: "MaxCapacityGrams",
                table: "Mixers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BatchCycleTime",
                table: "MixerBackbones",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxCapacityGrams",
                table: "Mixers");

            migrationBuilder.DropColumn(
                name: "BatchCycleTime",
                table: "MixerBackbones");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WIPTankLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TankCode",
                table: "WIPTankLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SKUs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Presentation",
                table: "SKUs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "ProductionScheduleItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductionPlans",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlanCode",
                table: "ProductionPlans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductionLines",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductionLines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LineCode",
                table: "ProductionLines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ProductionLines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Mixers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Mixers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Mixers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledDate",
                table: "MixerBackbones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentItemIndex",
                table: "LineProductionSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TankCode",
                table: "BIGWIPTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Backbones",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
