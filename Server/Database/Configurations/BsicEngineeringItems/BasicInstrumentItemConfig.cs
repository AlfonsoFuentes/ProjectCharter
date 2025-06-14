using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;


namespace Server.Database.Configurations.BsicEngineeringItems
{
    internal class BasicInstrumentItemConfig : IEntityTypeConfiguration<BasicInstrumentItem>
    {
        public void Configure(EntityTypeBuilder<BasicInstrumentItem> builder)
        {
            builder.HasOne(x => x.InstrumentTemplate)
                     .WithMany(t => t.BasicInstruments)
                     .HasForeignKey(e => e.BasicInstrumentTemplateId)

         .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
