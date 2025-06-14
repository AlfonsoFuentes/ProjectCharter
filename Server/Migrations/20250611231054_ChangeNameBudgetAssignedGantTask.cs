using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameBudgetAssignedGantTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BudgetAssigned",
                table: "BudgetItemNewGantTasks",
                newName: "GanttTaskBudgetAssigned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GanttTaskBudgetAssigned",
                table: "BudgetItemNewGantTasks",
                newName: "BudgetAssigned");
        }
    }
}
