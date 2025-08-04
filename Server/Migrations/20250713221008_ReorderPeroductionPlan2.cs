using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderPeroductionPlan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionLineAssignmentId",
                table: "ProductionScheduleItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionLineId",
                table: "ProductionLineAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionScheduleItems_ProductionLineAssignmentId",
                table: "ProductionScheduleItems",
                column: "ProductionLineAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLineAssignments_ProductionLineId",
                table: "ProductionLineAssignments",
                column: "ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionLineAssignments_ProductionLines_ProductionLineId",
                table: "ProductionLineAssignments",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionScheduleItems_ProductionLineAssignments_ProductionLineAssignmentId",
                table: "ProductionScheduleItems",
                column: "ProductionLineAssignmentId",
                principalTable: "ProductionLineAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionLineAssignments_ProductionLines_ProductionLineId",
                table: "ProductionLineAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionScheduleItems_ProductionLineAssignments_ProductionLineAssignmentId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductionScheduleItems_ProductionLineAssignmentId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductionLineAssignments_ProductionLineId",
                table: "ProductionLineAssignments");

            migrationBuilder.DropColumn(
                name: "ProductionLineAssignmentId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "ProductionLineId",
                table: "ProductionLineAssignments");
        }
    }
}
