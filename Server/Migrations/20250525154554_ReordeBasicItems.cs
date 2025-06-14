using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReordeBasicItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_EquipmentTemplates_EquipmentTemplateId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Instruments_InstrumentTemplates_InstrumentTemplateId",
                table: "Instruments");

            migrationBuilder.DropForeignKey(
                name: "FK_Isometrics_EngineeringFluidCodes_FluidCodeId",
                table: "Isometrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Isometrics_PipeTemplates_PipeTemplateId",
                table: "Isometrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Valves_ValveTemplates_ValveTemplateId",
                table: "Valves");

            migrationBuilder.DropIndex(
                name: "IX_Valves_ValveTemplateId",
                table: "Valves");

            migrationBuilder.DropIndex(
                name: "IX_Nozzles_EngineeringItemId",
                table: "Nozzles");

            migrationBuilder.DropIndex(
                name: "IX_Nozzles_ItemConnectedId",
                table: "Nozzles");

            migrationBuilder.DropIndex(
                name: "IX_Isometrics_FluidCodeId",
                table: "Isometrics");

            migrationBuilder.DropIndex(
                name: "IX_Isometrics_PipeTemplateId",
                table: "Isometrics");

            migrationBuilder.DropIndex(
                name: "IX_Instruments_InstrumentTemplateId",
                table: "Instruments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_EquipmentTemplateId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "IsExisting",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "ProvisionalTag",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "TagLetter",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "ValveTemplateId",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "EngineeringItemId",
                table: "Nozzles");

            migrationBuilder.DropColumn(
                name: "ItemConnectedId",
                table: "Nozzles");

            migrationBuilder.DropColumn(
                name: "FluidCodeId",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "IsExisting",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "LaborQuantity",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "MaterialQuantity",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "PipeTemplateId",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "ProvisionalTag",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "TagLetter",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "InstrumentTemplateId",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "IsExisting",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "ProvisionalTag",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "TagLetter",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "EquipmentTemplateId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "IsExisting",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ProvisionalTag",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "TagLetter",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "Equipments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExisting",
                table: "Valves",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProvisionalTag",
                table: "Valves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Valves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagLetter",
                table: "Valves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "Valves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ValveTemplateId",
                table: "Valves",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EngineeringItemId",
                table: "Nozzles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemConnectedId",
                table: "Nozzles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FluidCodeId",
                table: "Isometrics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExisting",
                table: "Isometrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "LaborQuantity",
                table: "Isometrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaterialQuantity",
                table: "Isometrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "PipeTemplateId",
                table: "Isometrics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvisionalTag",
                table: "Isometrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagLetter",
                table: "Isometrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "Isometrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "InstrumentTemplateId",
                table: "Instruments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExisting",
                table: "Instruments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProvisionalTag",
                table: "Instruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Instruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagLetter",
                table: "Instruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "Instruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentTemplateId",
                table: "Equipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExisting",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProvisionalTag",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagLetter",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Valves_ValveTemplateId",
                table: "Valves",
                column: "ValveTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Nozzles_EngineeringItemId",
                table: "Nozzles",
                column: "EngineeringItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Nozzles_ItemConnectedId",
                table: "Nozzles",
                column: "ItemConnectedId");

            migrationBuilder.CreateIndex(
                name: "IX_Isometrics_FluidCodeId",
                table: "Isometrics",
                column: "FluidCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Isometrics_PipeTemplateId",
                table: "Isometrics",
                column: "PipeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_InstrumentTemplateId",
                table: "Instruments",
                column: "InstrumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTemplateId",
                table: "Equipments",
                column: "EquipmentTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_EquipmentTemplates_EquipmentTemplateId",
                table: "Equipments",
                column: "EquipmentTemplateId",
                principalTable: "EquipmentTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instruments_InstrumentTemplates_InstrumentTemplateId",
                table: "Instruments",
                column: "InstrumentTemplateId",
                principalTable: "InstrumentTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Isometrics_EngineeringFluidCodes_FluidCodeId",
                table: "Isometrics",
                column: "FluidCodeId",
                principalTable: "EngineeringFluidCodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Isometrics_PipeTemplates_PipeTemplateId",
                table: "Isometrics",
                column: "PipeTemplateId",
                principalTable: "PipeTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Valves_ValveTemplates_ValveTemplateId",
                table: "Valves",
                column: "ValveTemplateId",
                principalTable: "ValveTemplates",
                principalColumn: "Id");
        }
    }
}
