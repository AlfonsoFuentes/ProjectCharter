using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderInitialLevelBigWIp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitialLevelBigWips_ProductionScheduleItems_ProductionScheduleItemId",
                table: "InitialLevelBigWips");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialLevelWips_ProductionScheduleItems_ProductionScheduleItemId",
                table: "InitialLevelWips");

            migrationBuilder.RenameColumn(
                name: "ProductionScheduleItemId",
                table: "InitialLevelWips",
                newName: "ProductionLineAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialLevelWips_ProductionScheduleItemId",
                table: "InitialLevelWips",
                newName: "IX_InitialLevelWips_ProductionLineAssignmentId");

            migrationBuilder.RenameColumn(
                name: "ProductionScheduleItemId",
                table: "InitialLevelBigWips",
                newName: "ProductionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialLevelBigWips_ProductionScheduleItemId",
                table: "InitialLevelBigWips",
                newName: "IX_InitialLevelBigWips_ProductionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_InitialLevelBigWips_ProductionPlans_ProductionPlanId",
                table: "InitialLevelBigWips",
                column: "ProductionPlanId",
                principalTable: "ProductionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialLevelWips_ProductionLineAssignments_ProductionLineAssignmentId",
                table: "InitialLevelWips",
                column: "ProductionLineAssignmentId",
                principalTable: "ProductionLineAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitialLevelBigWips_ProductionPlans_ProductionPlanId",
                table: "InitialLevelBigWips");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialLevelWips_ProductionLineAssignments_ProductionLineAssignmentId",
                table: "InitialLevelWips");

            migrationBuilder.RenameColumn(
                name: "ProductionLineAssignmentId",
                table: "InitialLevelWips",
                newName: "ProductionScheduleItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialLevelWips_ProductionLineAssignmentId",
                table: "InitialLevelWips",
                newName: "IX_InitialLevelWips_ProductionScheduleItemId");

            migrationBuilder.RenameColumn(
                name: "ProductionPlanId",
                table: "InitialLevelBigWips",
                newName: "ProductionScheduleItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialLevelBigWips_ProductionPlanId",
                table: "InitialLevelBigWips",
                newName: "IX_InitialLevelBigWips_ProductionScheduleItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InitialLevelBigWips_ProductionScheduleItems_ProductionScheduleItemId",
                table: "InitialLevelBigWips",
                column: "ProductionScheduleItemId",
                principalTable: "ProductionScheduleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialLevelWips_ProductionScheduleItems_ProductionScheduleItemId",
                table: "InitialLevelWips",
                column: "ProductionScheduleItemId",
                principalTable: "ProductionScheduleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
