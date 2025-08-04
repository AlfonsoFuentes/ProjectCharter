using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFormatChangeSKU : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
