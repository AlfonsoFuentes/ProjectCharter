using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class orderLineSpeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LineSpeeds",
                table: "LineSpeeds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineSpeeds",
                table: "LineSpeeds",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LineSpeeds_LineId",
                table: "LineSpeeds",
                column: "LineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LineSpeeds",
                table: "LineSpeeds");

            migrationBuilder.DropIndex(
                name: "IX_LineSpeeds_LineId",
                table: "LineSpeeds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineSpeeds",
                table: "LineSpeeds",
                columns: new[] { "LineId", "SkuId" });
        }
    }
}
