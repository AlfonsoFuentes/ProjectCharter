using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.ProjectManagements;


namespace Server.Database.Configurations
{
    internal class DeliverableConfig : IEntityTypeConfiguration<Deliverable>
    {
        public void Configure(EntityTypeBuilder<Deliverable> builder)
        {
            builder.HasKey(ci => ci.Id);

          

            builder
         .HasMany(m => m.NewGanttTasks) // Un hito tiene un padre
         .WithOne(m => m.Deliverable) // Un padre puede tener muchos subhitos
         .HasForeignKey(m => m.DeliverableId) // Clave foránea
         .OnDelete(DeleteBehavior.Cascade); // Evita la eliminación en cascada para evitar problemas
        }

    }
}
