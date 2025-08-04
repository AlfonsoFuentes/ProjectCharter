using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderPeroductionPlan3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SkuId",
                table: "ProductionScheduleItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionScheduleItems_SkuId",
                table: "ProductionScheduleItems",
                column: "SkuId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionScheduleItems_SKUs_SkuId",
                table: "ProductionScheduleItems",
                column: "SkuId",
                principalTable: "SKUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionScheduleItems_SKUs_SkuId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductionScheduleItems_SkuId",
                table: "ProductionScheduleItems");

            migrationBuilder.DropColumn(
                name: "SkuId",
                table: "ProductionScheduleItems");
        }
    }
}
