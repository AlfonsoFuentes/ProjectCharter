using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReviewNamesGantTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "PlannedStartDate",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "IsExpanded",
                table: "Deliverables");

            migrationBuilder.RenameColumn(
                name: "RealStartDate",
                table: "NewGanttTasks",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "RealEndDate",
                table: "NewGanttTasks",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "LagUnits",
                table: "NewGanttTasks",
                newName: "LagUnit");

            migrationBuilder.RenameColumn(
                name: "LagDays",
                table: "NewGanttTasks",
                newName: "LagInUnits");

            migrationBuilder.RenameColumn(
                name: "DurationUnits",
                table: "NewGanttTasks",
                newName: "DurationUnit");

            migrationBuilder.RenameColumn(
                name: "DurationDays",
                table: "NewGanttTasks",
                newName: "LagInDays");

            migrationBuilder.RenameColumn(
                name: "PlannedStartDate",
                table: "Deliverables",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "PlannedEndDate",
                table: "Deliverables",
                newName: "EndDate");

            migrationBuilder.AddColumn<double>(
                name: "DurationInDays",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurationInUnit",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurationInDays",
                table: "Deliverables",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurationInUnit",
                table: "Deliverables",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DurationUnit",
                table: "Deliverables",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "DurationInUnit",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "Deliverables");

            migrationBuilder.DropColumn(
                name: "DurationInUnit",
                table: "Deliverables");

            migrationBuilder.DropColumn(
                name: "DurationUnit",
                table: "Deliverables");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "NewGanttTasks",
                newName: "RealStartDate");

            migrationBuilder.RenameColumn(
                name: "LagUnit",
                table: "NewGanttTasks",
                newName: "LagUnits");

            migrationBuilder.RenameColumn(
                name: "LagInUnits",
                table: "NewGanttTasks",
                newName: "LagDays");

            migrationBuilder.RenameColumn(
                name: "LagInDays",
                table: "NewGanttTasks",
                newName: "DurationDays");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "NewGanttTasks",
                newName: "RealEndDate");

            migrationBuilder.RenameColumn(
                name: "DurationUnit",
                table: "NewGanttTasks",
                newName: "DurationUnits");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Deliverables",
                newName: "PlannedStartDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Deliverables",
                newName: "PlannedEndDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpanded",
                table: "Deliverables",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
