using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderDeliverablreTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpanded",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "WBS",
                table: "NewGanttTasks");

            migrationBuilder.RenameColumn(
                name: "LabelOrder",
                table: "NewGanttTasks",
                newName: "MainOrder");

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "NewGanttTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "Deliverables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MainOrder",
                table: "Deliverables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "NewGanttTasks");

            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "Deliverables");

            migrationBuilder.DropColumn(
                name: "MainOrder",
                table: "Deliverables");

            migrationBuilder.RenameColumn(
                name: "MainOrder",
                table: "NewGanttTasks",
                newName: "LabelOrder");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpanded",
                table: "NewGanttTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WBS",
                table: "NewGanttTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
