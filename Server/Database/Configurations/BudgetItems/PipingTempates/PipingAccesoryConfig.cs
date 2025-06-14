using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings;


namespace Server.Database.Configurations.BudgetItems.PipingTempates
{
    internal class PipingAccesoryConfig : IEntityTypeConfiguration<PipingAccesory>
    {
        public void Configure(EntityTypeBuilder<PipingAccesory> builder)
        {
            builder.HasOne(x => x.PipingCategory)
        .WithMany(t => t.PipingAccesories)
        .HasForeignKey(e => e.PipingCategoryId)
        .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(x => x.PipingConnectionTypes)
                .WithOne(t => t.PipingAccesory).HasForeignKey(e => e.PipingAccesoryId).IsRequired().OnDelete(DeleteBehavior.Cascade);


        }
    }
}
