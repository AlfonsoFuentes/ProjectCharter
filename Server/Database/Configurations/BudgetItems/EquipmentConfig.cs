using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Equipments;


namespace Server.Database.Configurations.BudgetItems
{
    internal class EquipmentConfig : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
         //   builder.HasOne(x => x.EquipmentTemplate)
         //            .WithMany(t => t.Equipments)
         //            .HasForeignKey(e => e.EquipmentTemplateId)

         //.OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(x => x.EquipmentItems)
            // .WithOne(t => t.Equipment)
            // .HasForeignKey(e => e.EquipmentId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
