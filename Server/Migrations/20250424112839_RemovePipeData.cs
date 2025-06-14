using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovePipeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diameter",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "EquivalentLenghPrice",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "Insulation",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "LaborDayPrice",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Isometrics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Diameter",
                table: "Isometrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "EquivalentLenghPrice",
                table: "Isometrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Insulation",
                table: "Isometrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "LaborDayPrice",
                table: "Isometrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Material",
                table: "Isometrics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
