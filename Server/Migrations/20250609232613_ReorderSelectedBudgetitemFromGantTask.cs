using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderSelectedBudgetitemFromGantTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItemNewGantTasks_SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks",
                column: "SelectedBasicEngineeringItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetItemNewGantTasks_SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropColumn(
                name: "SelectedBasicEngineeringItemId",
                table: "BudgetItemNewGantTasks");
        }
    }
}
