using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRealStateTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RealDurationInDays",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RealDurationInUnit",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RealDurationUnit",
                table: "NewGanttTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealEndDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RealStartDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealDurationInDays",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "RealDurationInUnit",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "RealDurationUnit",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "RealEndDate",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "RealStartDate",
                table: "NewGanttTasks");
        }
    }
}
