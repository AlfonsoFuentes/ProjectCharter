using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Reorderformatchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatChangeTime",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "FormatChangeTimeUnit",
                table: "SKUs");

            migrationBuilder.AddColumn<double>(
                name: "FormatChangeTime",
                table: "ProductionLines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FormatChangeTimeUnit",
                table: "ProductionLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatChangeTime",
                table: "ProductionLines");

            migrationBuilder.DropColumn(
                name: "FormatChangeTimeUnit",
                table: "ProductionLines");

            migrationBuilder.AddColumn<double>(
                name: "FormatChangeTime",
                table: "SKUs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FormatChangeTimeUnit",
                table: "SKUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
