using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;


namespace Server.Database.Configurations.BsicEngineeringItems
{
    internal class BasicValveItemConfig : IEntityTypeConfiguration<BasicValveItem>
    {
        public void Configure(EntityTypeBuilder<BasicValveItem> builder)
        {
            builder.HasOne(x => x.ValveTemplate)
                     .WithMany(t => t.BasicValves)
                     .HasForeignKey(e => e.BasicValveTemplateId)

         .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
