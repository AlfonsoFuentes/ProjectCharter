using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicEngineering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BasicEngineeringItemConnectedId",
                table: "Nozzles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BasicEngineeringItemId",
                table: "Nozzles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BasicPipeItemId",
                table: "IsometricItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BasicEquipmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvisionalTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExisting = table.Column<bool>(type: "bit", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicEquipmentTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_BasicEquipmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicEquipmentItems_EquipmentTemplates_BasicEquipmentTemplateId",
                        column: x => x.BasicEquipmentTemplateId,
                        principalTable: "EquipmentTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BasicInstrumentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvisionalTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExisting = table.Column<bool>(type: "bit", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicInstrumentTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_BasicInstrumentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicInstrumentItems_InstrumentTemplates_BasicInstrumentTemplateId",
                        column: x => x.BasicInstrumentTemplateId,
                        principalTable: "InstrumentTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BasicPipeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvisionalTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExisting = table.Column<bool>(type: "bit", nullable: false),
                    BasicFluidCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BasicPipeTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialQuantity = table.Column<double>(type: "float", nullable: false),
                    LaborQuantity = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_BasicPipeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPipeItems_EngineeringFluidCodes_BasicFluidCodeId",
                        column: x => x.BasicFluidCodeId,
                        principalTable: "EngineeringFluidCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BasicPipeItems_PipeTemplates_BasicPipeTemplateId",
                        column: x => x.BasicPipeTemplateId,
                        principalTable: "PipeTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BasicValveItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvisionalTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExisting = table.Column<bool>(type: "bit", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicValveTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_BasicValveItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicValveItems_ValveTemplates_BasicValveTemplateId",
                        column: x => x.BasicValveTemplateId,
                        principalTable: "ValveTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nozzles_BasicEngineeringItemConnectedId",
                table: "Nozzles",
                column: "BasicEngineeringItemConnectedId");

            migrationBuilder.CreateIndex(
                name: "IX_Nozzles_BasicEngineeringItemId",
                table: "Nozzles",
                column: "BasicEngineeringItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IsometricItems_BasicPipeItemId",
                table: "IsometricItems",
                column: "BasicPipeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicEquipmentItems_BasicEquipmentTemplateId",
                table: "BasicEquipmentItems",
                column: "BasicEquipmentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInstrumentItems_BasicInstrumentTemplateId",
                table: "BasicInstrumentItems",
                column: "BasicInstrumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPipeItems_BasicFluidCodeId",
                table: "BasicPipeItems",
                column: "BasicFluidCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPipeItems_BasicPipeTemplateId",
                table: "BasicPipeItems",
                column: "BasicPipeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicValveItems_BasicValveTemplateId",
                table: "BasicValveItems",
                column: "BasicValveTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_IsometricItems_BasicPipeItems_BasicPipeItemId",
                table: "IsometricItems",
                column: "BasicPipeItemId",
                principalTable: "BasicPipeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IsometricItems_BasicPipeItems_BasicPipeItemId",
                table: "IsometricItems");

            migrationBuilder.DropTable(
                name: "BasicEquipmentItems");

            migrationBuilder.DropTable(
                name: "BasicInstrumentItems");

            migrationBuilder.DropTable(
                name: "BasicPipeItems");

            migrationBuilder.DropTable(
                name: "BasicValveItems");

            migrationBuilder.DropIndex(
                name: "IX_Nozzles_BasicEngineeringItemConnectedId",
                table: "Nozzles");

            migrationBuilder.DropIndex(
                name: "IX_Nozzles_BasicEngineeringItemId",
                table: "Nozzles");

            migrationBuilder.DropIndex(
                name: "IX_IsometricItems_BasicPipeItemId",
                table: "IsometricItems");

            migrationBuilder.DropColumn(
                name: "BasicEngineeringItemConnectedId",
                table: "Nozzles");

            migrationBuilder.DropColumn(
                name: "BasicEngineeringItemId",
                table: "Nozzles");

            migrationBuilder.DropColumn(
                name: "BasicPipeItemId",
                table: "IsometricItems");
        }
    }
}
