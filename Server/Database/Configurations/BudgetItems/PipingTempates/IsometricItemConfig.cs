using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings;


namespace Server.Database.Configurations.BudgetItems.PipingTempates
{
    internal class IsometricItemConfig : IEntityTypeConfiguration<IsometricItem>
    {
        public void Configure(EntityTypeBuilder<IsometricItem> builder)
        {
            builder.HasOne(x => x.PipingAccesory)
        .WithMany(t => t.IsometricItems)
        .HasForeignKey(e => e.PipingAccesoryId)

        .OnDelete(DeleteBehavior.NoAction);




        }
    }
}
