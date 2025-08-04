using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialLevelWIp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitialLevelWips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WIPTankLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionScheduleItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitialLevel = table.Column<double>(type: "float", nullable: false),
                    InitialLevelUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_InitialLevelWips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitialLevelWips_ProductionScheduleItems_ProductionScheduleItemId",
                        column: x => x.ProductionScheduleItemId,
                        principalTable: "ProductionScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InitialLevelWips_WIPTankLines_WIPTankLineId",
                        column: x => x.WIPTankLineId,
                        principalTable: "WIPTankLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InitialLevelWips_ProductionScheduleItemId",
                table: "InitialLevelWips",
                column: "ProductionScheduleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialLevelWips_WIPTankLineId",
                table: "InitialLevelWips",
                column: "WIPTankLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitialLevelWips");
        }
    }
}
