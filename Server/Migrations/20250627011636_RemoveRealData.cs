using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRealData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealEndDate",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "RealStartDate",
                table: "NewGanttTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RealEndDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealStartDate",
                table: "NewGanttTasks",
                type: "datetime2",
                nullable: true);
        }
    }
}
