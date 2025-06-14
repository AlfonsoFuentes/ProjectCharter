using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationToSelectedBasicItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicValveItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicPipeItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasicValveItems_BudgetItemNewGanttTaskId",
                table: "BasicValveItems",
                column: "BudgetItemNewGanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPipeItems_BudgetItemNewGanttTaskId",
                table: "BasicPipeItems",
                column: "BudgetItemNewGanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInstrumentItems_BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems",
                column: "BudgetItemNewGanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicEquipmentItems_BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems",
                column: "BudgetItemNewGanttTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicEquipmentItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems",
                column: "BudgetItemNewGanttTaskId",
                principalTable: "BudgetItemNewGantTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicInstrumentItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems",
                column: "BudgetItemNewGanttTaskId",
                principalTable: "BudgetItemNewGantTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPipeItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicPipeItems",
                column: "BudgetItemNewGanttTaskId",
                principalTable: "BudgetItemNewGantTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicValveItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicValveItems",
                column: "BudgetItemNewGanttTaskId",
                principalTable: "BudgetItemNewGantTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicEquipmentItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicInstrumentItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPipeItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicPipeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicValveItems_BudgetItemNewGantTasks_BudgetItemNewGanttTaskId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicValveItems_BudgetItemNewGanttTaskId",
                table: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicPipeItems_BudgetItemNewGanttTaskId",
                table: "BasicPipeItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicInstrumentItems_BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicEquipmentItems_BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems");

            migrationBuilder.DropColumn(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicValveItems");

            migrationBuilder.DropColumn(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicPipeItems");

            migrationBuilder.DropColumn(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicInstrumentItems");

            migrationBuilder.DropColumn(
                name: "BudgetItemNewGanttTaskId",
                table: "BasicEquipmentItems");
        }
    }
}
