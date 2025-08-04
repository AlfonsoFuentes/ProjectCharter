using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderPeroductionPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionScheduleItems_LineProductionSchedules_ScheduleId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionScheduleItems_SKUs_SkuId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropTable(
                name: "LineProductionSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ProductionScheduleItems_ScheduleId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductionScheduleItems_SkuId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "SkuId",
                table: "ProductionScheduleItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
                table: "ProductionScheduleItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SkuId",
                table: "ProductionScheduleItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LineProductionSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineProductionSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineProductionSchedules_ProductionLineAssignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "ProductionLineAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionScheduleItems_ScheduleId",
                table: "ProductionScheduleItems",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionScheduleItems_SkuId",
                table: "ProductionScheduleItems",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_LineProductionSchedules_AssignmentId",
                table: "LineProductionSchedules",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionScheduleItems_LineProductionSchedules_ScheduleId",
                table: "ProductionScheduleItems",
                column: "ScheduleId",
                principalTable: "LineProductionSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionScheduleItems_SKUs_SkuId",
                table: "ProductionScheduleItems",
                column: "SkuId",
                principalTable: "SKUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
