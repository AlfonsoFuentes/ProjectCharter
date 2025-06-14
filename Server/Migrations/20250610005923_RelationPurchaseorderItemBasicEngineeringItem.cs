using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RelationPurchaseorderItemBasicEngineeringItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BasicEngineeringItemId",
                table: "PurchaseOrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_BasicEngineeringItemId",
                table: "PurchaseOrderItems",
                column: "BasicEngineeringItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_BasicEngineeringItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "BasicEngineeringItemId",
                table: "PurchaseOrderItems");
        }
    }
}
