using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.PurchaseOrders;

namespace Server.Database.Configurations
{
    internal class PurchaseOrderItemConfig : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.HasKey(ci => ci.Id);

           
            builder.HasMany(c => c.PurchaseOrderReceiveds).WithOne(t => t.PurchaseOrderItem).HasForeignKey(e => e.PurchaseOrderItemId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

    }
}
