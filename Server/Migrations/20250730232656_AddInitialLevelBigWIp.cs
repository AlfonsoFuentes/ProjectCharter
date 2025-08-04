using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialLevelBigWIp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitialLevelBigWips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BIGWIPTankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_InitialLevelBigWips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitialLevelBigWips_BIGWIPTanks_BIGWIPTankId",
                        column: x => x.BIGWIPTankId,
                        principalTable: "BIGWIPTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InitialLevelBigWips_ProductionScheduleItems_ProductionScheduleItemId",
                        column: x => x.ProductionScheduleItemId,
                        principalTable: "ProductionScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InitialLevelBigWips_BIGWIPTankId",
                table: "InitialLevelBigWips",
                column: "BIGWIPTankId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialLevelBigWips_ProductionScheduleItemId",
                table: "InitialLevelBigWips",
                column: "ProductionScheduleItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitialLevelBigWips");
        }
    }
}
