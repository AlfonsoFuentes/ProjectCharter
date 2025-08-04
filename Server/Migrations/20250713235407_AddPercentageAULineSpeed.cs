using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPercentageAULineSpeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageAU",
                table: "ProductionScheduleItems");

            migrationBuilder.AddColumn<double>(
                name: "PercentageAU",
                table: "LineSpeeds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageAU",
                table: "LineSpeeds");

            migrationBuilder.AddColumn<double>(
                name: "PercentageAU",
                table: "ProductionScheduleItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
