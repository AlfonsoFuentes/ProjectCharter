using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBasicResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_BasicEngineeringItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_BudgetItemNewGantTasks_SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropColumn(
                name: "BasicEngineeringItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BasicEngineeringItemId",
                table: "PurchaseOrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_BasicEngineeringItemId",
                table: "PurchaseOrderItems",
                column: "BasicEngineeringItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItemNewGantTasks_SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks",
                column: "SelectedBasicEngineeringItemId");
        }
    }
}
