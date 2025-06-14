using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicEngineeringRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IsometricItems_Isometrics_IsometricId",
                table: "IsometricItems");

            migrationBuilder.DropIndex(
                name: "IX_IsometricItems_IsometricId",
                table: "IsometricItems");

            migrationBuilder.DropColumn(
                name: "IsometricId",
                table: "IsometricItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ValveId",
                table: "BasicValveItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InstrumentId",
                table: "BasicInstrumentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId",
                table: "BasicEquipmentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BasicValveItems_ValveId",
                table: "BasicValveItems",
                column: "ValveId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInstrumentItems_InstrumentId",
                table: "BasicInstrumentItems",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicEquipmentItems_EquipmentId",
                table: "BasicEquipmentItems",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicEquipmentItems_Equipments_EquipmentId",
                table: "BasicEquipmentItems",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicInstrumentItems_Instruments_InstrumentId",
                table: "BasicInstrumentItems",
                column: "InstrumentId",
                principalTable: "Instruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicValveItems_Valves_ValveId",
                table: "BasicValveItems",
                column: "ValveId",
                principalTable: "Valves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicEquipmentItems_Equipments_EquipmentId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInstrumentItems_Instruments_InstrumentId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicValveItems_Valves_ValveId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicValveItems_ValveId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicInstrumentItems_InstrumentId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicEquipmentItems_EquipmentId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropColumn(
                name: "ValveId",
                table: "BasicValveItems");

            migrationBuilder.DropColumn(
                name: "InstrumentId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "BasicEquipmentItems");

            migrationBuilder.AddColumn<Guid>(
                name: "IsometricId",
                table: "IsometricItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_IsometricItems_IsometricId",
                table: "IsometricItems",
                column: "IsometricId");

            migrationBuilder.AddForeignKey(
                name: "FK_IsometricItems_Isometrics_IsometricId",
                table: "IsometricItems",
                column: "IsometricId",
                principalTable: "Isometrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
