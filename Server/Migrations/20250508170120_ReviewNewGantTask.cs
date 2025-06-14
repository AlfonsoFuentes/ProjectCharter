using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReviewNewGantTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lag",
                table: "NewGanttTasks",
                newName: "LagUnits");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "NewGanttTasks",
                newName: "DurationUnits");

            migrationBuilder.AddColumn<double>(
                name: "DurationDays",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LagDays",
                table: "NewGanttTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "LagDays",
                table: "NewGanttTasks");

            migrationBuilder.RenameColumn(
                name: "LagUnits",
                table: "NewGanttTasks",
                newName: "Lag");

            migrationBuilder.RenameColumn(
                name: "DurationUnits",
                table: "NewGanttTasks",
                newName: "Duration");
        }
    }
}
