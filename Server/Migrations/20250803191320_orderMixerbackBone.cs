using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class orderMixerbackBone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MixerBackbones",
                table: "MixerBackbones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MixerBackbones",
                table: "MixerBackbones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MixerBackbones_MixerId",
                table: "MixerBackbones",
                column: "MixerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MixerBackbones",
                table: "MixerBackbones");

            migrationBuilder.DropIndex(
                name: "IX_MixerBackbones_MixerId",
                table: "MixerBackbones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MixerBackbones",
                table: "MixerBackbones",
                columns: new[] { "MixerId", "BackboneId" });
        }
    }
}
