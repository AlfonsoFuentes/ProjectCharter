using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ReviewPurchaseordersNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitaryValueCurrency",
                table: "PurchaseOrderItems",
                newName: "UnitaryValueQuoteCurrency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitaryValueQuoteCurrency",
                table: "PurchaseOrderItems",
                newName: "UnitaryValueCurrency");
        }
    }
}
