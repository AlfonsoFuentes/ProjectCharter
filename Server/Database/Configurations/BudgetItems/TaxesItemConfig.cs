using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.Taxes;

namespace Server.Database.Configurations.BudgetItems
{
    internal class TaxesItemConfig : IEntityTypeConfiguration<TaxesItem>
    {
        public void Configure(EntityTypeBuilder<TaxesItem> builder)
        {


            builder.HasOne(c => c.Selected)
                .WithMany(t => t.TaxesSelecteds)
                .HasForeignKey(x => x.SelectedId)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
