using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicEngineeringRelations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PipeId",
                table: "BasicPipeItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BasicPipeItems_PipeId",
                table: "BasicPipeItems",
                column: "PipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems",
                column: "PipeId",
                principalTable: "Isometrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPipeItems_Isometrics_PipeId",
                table: "BasicPipeItems");

            migrationBuilder.DropIndex(
                name: "IX_BasicPipeItems_PipeId",
                table: "BasicPipeItems");

            migrationBuilder.DropColumn(
                name: "PipeId",
                table: "BasicPipeItems");
        }
    }
}
