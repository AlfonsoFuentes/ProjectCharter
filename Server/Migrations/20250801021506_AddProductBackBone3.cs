using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductBackBone3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "SKUs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SKUs_ProductId",
                table: "SKUs",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SKUs_Products_ProductId",
                table: "SKUs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SKUs_Products_ProductId",
                table: "SKUs");

            migrationBuilder.DropIndex(
                name: "IX_SKUs_ProductId",
                table: "SKUs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SKUs");
        }
    }
}
