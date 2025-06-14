using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationToDependencyTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainTaskDependencys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DependencyTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DependencyType = table.Column<int>(type: "int", nullable: false),
                    LagUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LagInDays = table.Column<double>(type: "float", nullable: false),
                    LagInUnits = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_MainTaskDependencys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainTaskDependencys_NewGanttTasks_DependencyTaskId",
                        column: x => x.DependencyTaskId,
                        principalTable: "NewGanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainTaskDependencys_NewGanttTasks_MainTaskId",
                        column: x => x.MainTaskId,
                        principalTable: "NewGanttTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainTaskDependencys_DependencyTaskId",
                table: "MainTaskDependencys",
                column: "DependencyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MainTaskDependencys_MainTaskId",
                table: "MainTaskDependencys",
                column: "MainTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainTaskDependencys");
        }
    }
}
