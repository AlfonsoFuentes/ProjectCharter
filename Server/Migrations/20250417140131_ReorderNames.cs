using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReorderNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountAssigment",
                table: "PurchaseOrders",
                newName: "ProjectAccount");

            migrationBuilder.AddColumn<int>(
                name: "CostCenter",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MainBudgetItemId",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "MainBudgetItemId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "ProjectAccount",
                table: "PurchaseOrders",
                newName: "AccountAssigment");
        }
    }
}
