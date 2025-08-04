using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class FLSKUs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DentalCreamComponents");

            migrationBuilder.AddColumn<Guid>(
                name: "BackBoneId",
                table: "DentalCreamComponents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DentalCreamBackBones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalCreamBackBones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentalCreamComponents_BackBoneId",
                table: "DentalCreamComponents",
                column: "BackBoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_DentalCreamComponents_DentalCreamBackBones_BackBoneId",
                table: "DentalCreamComponents",
                column: "BackBoneId",
                principalTable: "DentalCreamBackBones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DentalCreamComponents_DentalCreamBackBones_BackBoneId",
                table: "DentalCreamComponents");

            migrationBuilder.DropTable(
                name: "DentalCreamBackBones");

            migrationBuilder.DropIndex(
                name: "IX_DentalCreamComponents_BackBoneId",
                table: "DentalCreamComponents");

            migrationBuilder.DropColumn(
                name: "BackBoneId",
                table: "DentalCreamComponents");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DentalCreamComponents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
