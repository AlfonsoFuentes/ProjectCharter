using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewGanttTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependencyList",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "DependencyType",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "LagInDays",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "LagInUnits",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "LagUnit",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "BudgetItemNewGantTasks");

            migrationBuilder.DropColumn(
                name: "PlannedStartDate",
                table: "BudgetItemNewGantTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DependencyList",
                table: "NewGanttTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependencyType",
                table: "NewGanttTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LagInDays",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LagInUnits",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "LagUnit",
                table: "NewGanttTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "BudgetItemNewGantTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDate",
                table: "BudgetItemNewGantTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDate",
                table: "BudgetItemNewGantTasks",
                type: "datetime2",
                nullable: true);
        }
    }
}
