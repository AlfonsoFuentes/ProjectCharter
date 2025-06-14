using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReordeBasicItems2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicEquipmentItems_Equipments_EquipmentId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInstrumentItems_Instruments_InstrumentId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicValveItems_Valves_ValveId",
                table: "BasicValveItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ValveId",
                table: "BasicValveItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BasicValveItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PipeId",
                table: "BasicPipeItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BasicPipeItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "InstrumentId",
                table: "BasicInstrumentItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BasicInstrumentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EquipmentId",
                table: "BasicEquipmentItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BasicEquipmentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BasicValveItems_ProjectId",
                table: "BasicValveItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPipeItems_ProjectId",
                table: "BasicPipeItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInstrumentItems_ProjectId",
                table: "BasicInstrumentItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicEquipmentItems_ProjectId",
                table: "BasicEquipmentItems",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicEquipmentItems_Equipments_EquipmentId",
                table: "BasicEquipmentItems",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicEquipmentItems_Projects_ProjectId",
                table: "BasicEquipmentItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicInstrumentItems_Instruments_InstrumentId",
                table: "BasicInstrumentItems",
                column: "InstrumentId",
                principalTable: "Instruments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicInstrumentItems_Projects_ProjectId",
                table: "BasicInstrumentItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems",
                column: "PipeId",
                principalTable: "Isometrics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPipeItems_Projects_ProjectId",
                table: "BasicPipeItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicValveItems_Projects_ProjectId",
                table: "BasicValveItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicValveItems_Valves_ValveId",
                table: "BasicValveItems",
                column: "ValveId",
                principalTable: "Valves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicEquipmentItems_Equipments_EquipmentId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicEquipmentItems_Projects_ProjectId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInstrumentItems_Instruments_InstrumentId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInstrumentItems_Projects_ProjectId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPipeItems_Projects_ProjectId",
                table: "BasicPipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicValveItems_Projects_ProjectId",
                table: "BasicValveItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicValveItems_Valves_ValveId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicValveItems_ProjectId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicPipeItems_ProjectId",
                table: "BasicPipeItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicInstrumentItems_ProjectId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicEquipmentItems_ProjectId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BasicValveItems");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BasicPipeItems");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BasicEquipmentItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ValveId",
                table: "BasicValveItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PipeId",
                table: "BasicPipeItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InstrumentId",
                table: "BasicInstrumentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EquipmentId",
                table: "BasicEquipmentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems",
                column: "PipeId",
                principalTable: "Isometrics",
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
    }
}
