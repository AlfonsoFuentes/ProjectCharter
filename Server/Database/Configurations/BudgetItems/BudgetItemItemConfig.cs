using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems;


namespace Server.Database.Configurations.BudgetItems
{
    internal class BudgetItemItemConfig : IEntityTypeConfiguration<BudgetItem>
    {
        public void Configure(EntityTypeBuilder<BudgetItem> builder)
        {

            builder.HasMany(x => x.PurchaseOrderItems)
                .WithOne(x => x.BudgetItem)
                .HasForeignKey(x => x.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
