using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReordeBasicItems3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BudgetUSD",
                table: "BasicValveItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BasicValveItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "BudgetUSD",
                table: "BasicPipeItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BasicPipeItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "BudgetUSD",
                table: "BasicInstrumentItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BasicInstrumentItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "BudgetUSD",
                table: "BasicEquipmentItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BasicEquipmentItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetUSD",
                table: "BasicValveItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BasicValveItems");

            migrationBuilder.DropColumn(
                name: "BudgetUSD",
                table: "BasicPipeItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BasicPipeItems");

            migrationBuilder.DropColumn(
                name: "BudgetUSD",
                table: "BasicInstrumentItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BasicInstrumentItems");

            migrationBuilder.DropColumn(
                name: "BudgetUSD",
                table: "BasicEquipmentItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BasicEquipmentItems");
        }
    }
}
