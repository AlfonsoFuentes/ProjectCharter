using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.Taxes;

namespace Server.Database.Configurations.BudgetItems
{
    internal class TaxConfig : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {

            builder.HasMany(x => x.TaxesItems)
          .WithOne(t => t.TaxItem)
          .HasForeignKey(e => e.TaxItemId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
