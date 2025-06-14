using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RefactorizingGanttTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alterations_GanttTasks_GanttTaskId",
                table: "Alterations");

            migrationBuilder.DropForeignKey(
                name: "FK_Contingencys_GanttTasks_GanttTaskId",
                table: "Contingencys");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableResources_GanttTasks_GanttTaskId",
                table: "DeliverableResources");

            migrationBuilder.DropForeignKey(
                name: "FK_EHSs_GanttTasks_GanttTaskId",
                table: "EHSs");

            migrationBuilder.DropForeignKey(
                name: "FK_Electricals_GanttTasks_GanttTaskId",
                table: "Electricals");

            migrationBuilder.DropForeignKey(
                name: "FK_Engineerings_GanttTasks_GanttTaskId",
                table: "Engineerings");

            migrationBuilder.DropForeignKey(
                name: "FK_EngineeringSalarys_GanttTasks_GanttTaskId",
                table: "EngineeringSalarys");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_GanttTasks_GanttTaskId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Foundations_GanttTasks_GanttTaskId",
                table: "Foundations");

            migrationBuilder.DropForeignKey(
                name: "FK_Instruments_GanttTasks_GanttTaskId",
                table: "Instruments");

            migrationBuilder.DropForeignKey(
                name: "FK_Isometrics_GanttTasks_GanttTaskId",
                table: "Isometrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Paintings_GanttTasks_GanttTaskId",
                table: "Paintings");

            migrationBuilder.DropForeignKey(
                name: "FK_Structurals_GanttTasks_GanttTaskId",
                table: "Structurals");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_GanttTasks_GanttTaskId",
                table: "Taxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Testings_GanttTasks_GanttTaskId",
                table: "Testings");

            migrationBuilder.DropForeignKey(
                name: "FK_Valves_GanttTasks_GanttTaskId",
                table: "Valves");

            migrationBuilder.DropTable(
                name: "GanttTasks");

            migrationBuilder.DropTable(
                name: "PublisherObservers");

            migrationBuilder.DropIndex(
                name: "IX_Valves_GanttTaskId",
                table: "Valves");

            migrationBuilder.DropIndex(
                name: "IX_Testings_GanttTaskId",
                table: "Testings");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_GanttTaskId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Structurals_GanttTaskId",
                table: "Structurals");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_GanttTaskId",
                table: "Paintings");

            migrationBuilder.DropIndex(
                name: "IX_Isometrics_GanttTaskId",
                table: "Isometrics");

            migrationBuilder.DropIndex(
                name: "IX_Instruments_GanttTaskId",
                table: "Instruments");

            migrationBuilder.DropIndex(
                name: "IX_Foundations_GanttTaskId",
                table: "Foundations");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_GanttTaskId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_EngineeringSalarys_GanttTaskId",
                table: "EngineeringSalarys");

            migrationBuilder.DropIndex(
                name: "IX_Engineerings_GanttTaskId",
                table: "Engineerings");

            migrationBuilder.DropIndex(
                name: "IX_Electricals_GanttTaskId",
                table: "Electricals");

            migrationBuilder.DropIndex(
                name: "IX_EHSs_GanttTaskId",
                table: "EHSs");

            migrationBuilder.DropIndex(
                name: "IX_DeliverableResources_GanttTaskId",
                table: "DeliverableResources");

            migrationBuilder.DropIndex(
                name: "IX_Contingencys_GanttTaskId",
                table: "Contingencys");

            migrationBuilder.DropIndex(
                name: "IX_Alterations_GanttTaskId",
                table: "Alterations");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Valves");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Testings");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Structurals");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Isometrics");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Foundations");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "EngineeringSalarys");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Engineerings");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Electricals");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "EHSs");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "DeliverableResources");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Contingencys");

            migrationBuilder.DropColumn(
                name: "GanttTaskId",
                table: "Alterations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Valves",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Testings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Taxes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Structurals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Paintings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Isometrics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Instruments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Foundations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Equipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "EngineeringSalarys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Engineerings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Electricals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "EHSs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "DeliverableResources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Contingencys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GanttTaskId",
                table: "Alterations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GanttTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliverableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DependentantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DependencyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsExpanded = table.Column<bool>(type: "bit", nullable: false),
                    LabelOrder = table.Column<int>(type: "int", nullable: false),
                    Lag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RealEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RealStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowBudgetItems = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WBS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GanttTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GanttTasks_Deliverables_DeliverableId",
                        column: x => x.DeliverableId,
                        principalTable: "Deliverables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GanttTasks_GanttTasks_DependentantId",
                        column: x => x.DependentantId,
                        principalTable: "GanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GanttTasks_GanttTasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "GanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublisherObservers",
                columns: table => new
                {
                    PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObserverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherObservers", x => new { x.PublisherId, x.ObserverId });
                    table.ForeignKey(
                        name: "FK_PublisherObservers_NewGanttTasks_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "NewGanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublisherObservers_NewGanttTasks_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "NewGanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Valves_GanttTaskId",
                table: "Valves",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Testings_GanttTaskId",
                table: "Testings",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_GanttTaskId",
                table: "Taxes",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Structurals_GanttTaskId",
                table: "Structurals",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_GanttTaskId",
                table: "Paintings",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Isometrics_GanttTaskId",
                table: "Isometrics",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_GanttTaskId",
                table: "Instruments",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Foundations_GanttTaskId",
                table: "Foundations",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_GanttTaskId",
                table: "Equipments",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringSalarys_GanttTaskId",
                table: "EngineeringSalarys",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Engineerings_GanttTaskId",
                table: "Engineerings",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Electricals_GanttTaskId",
                table: "Electricals",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_EHSs_GanttTaskId",
                table: "EHSs",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliverableResources_GanttTaskId",
                table: "DeliverableResources",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Contingencys_GanttTaskId",
                table: "Contingencys",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Alterations_GanttTaskId",
                table: "Alterations",
                column: "GanttTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GanttTasks_DeliverableId",
                table: "GanttTasks",
                column: "DeliverableId");

            migrationBuilder.CreateIndex(
                name: "IX_GanttTasks_DependentantId",
                table: "GanttTasks",
                column: "DependentantId");

            migrationBuilder.CreateIndex(
                name: "IX_GanttTasks_ParentId",
                table: "GanttTasks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherObservers_ObserverId",
                table: "PublisherObservers",
                column: "ObserverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alterations_GanttTasks_GanttTaskId",
                table: "Alterations",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contingencys_GanttTasks_GanttTaskId",
                table: "Contingencys",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableResources_GanttTasks_GanttTaskId",
                table: "DeliverableResources",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EHSs_GanttTasks_GanttTaskId",
                table: "EHSs",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Electricals_GanttTasks_GanttTaskId",
                table: "Electricals",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Engineerings_GanttTasks_GanttTaskId",
                table: "Engineerings",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EngineeringSalarys_GanttTasks_GanttTaskId",
                table: "EngineeringSalarys",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_GanttTasks_GanttTaskId",
                table: "Equipments",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Foundations_GanttTasks_GanttTaskId",
                table: "Foundations",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Instruments_GanttTasks_GanttTaskId",
                table: "Instruments",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Isometrics_GanttTasks_GanttTaskId",
                table: "Isometrics",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paintings_GanttTasks_GanttTaskId",
                table: "Paintings",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Structurals_GanttTasks_GanttTaskId",
                table: "Structurals",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_GanttTasks_GanttTaskId",
                table: "Taxes",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Testings_GanttTasks_GanttTaskId",
                table: "Testings",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Valves_GanttTasks_GanttTaskId",
                table: "Valves",
                column: "GanttTaskId",
                principalTable: "GanttTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
