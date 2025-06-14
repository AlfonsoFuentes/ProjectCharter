using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGanttTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "GanttTasks",
                newName: "RealStartDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "GanttTasks",
                newName: "RealEndDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDate",
                table: "GanttTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDate",
                table: "GanttTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "GanttTasks");

            migrationBuilder.DropColumn(
                name: "PlannedStartDate",
                table: "GanttTasks");

            migrationBuilder.RenameColumn(
                name: "RealStartDate",
                table: "GanttTasks",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "RealEndDate",
                table: "GanttTasks",
                newName: "EndDate");
        }
    }
}
