using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ImproveBudgetItemRelationManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemNewGantTasks_NewGanttTasks_BudgetItemId",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItemNewGantTasks",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BudgetItemNewGantTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItemNewGantTasks",
                table: "BudgetItemNewGantTasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItemNewGantTasks_BudgetItemId",
                table: "BudgetItemNewGantTasks",
                column: "BudgetItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemNewGantTasks_NewGanttTasks_NewGanttTaskId",
                table: "BudgetItemNewGantTasks",
                column: "NewGanttTaskId",
                principalTable: "NewGanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemNewGantTasks_NewGanttTasks_NewGanttTaskId",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItemNewGantTasks",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropIndex(
                name: "IX_BudgetItemNewGantTasks_BudgetItemId",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItemNewGantTasks",
                table: "BudgetItemNewGantTasks",
                columns: new[] { "BudgetItemId", "NewGanttTaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemNewGantTasks_NewGanttTasks_BudgetItemId",
                table: "BudgetItemNewGantTasks",
                column: "BudgetItemId",
                principalTable: "NewGanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
