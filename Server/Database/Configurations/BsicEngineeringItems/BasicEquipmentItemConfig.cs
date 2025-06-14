using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;


namespace Server.Database.Configurations.BsicEngineeringItems
{
    internal class BasicEquipmentItemConfig : IEntityTypeConfiguration<BasicEquipmentItem>
    {
        public void Configure(EntityTypeBuilder<BasicEquipmentItem> builder)
        {
            builder.HasOne(x => x.EquipmentTemplate)
                     .WithMany(t => t.BasicEquipments)
                     .HasForeignKey(e => e.BasicEquipmentTemplateId)
         .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
