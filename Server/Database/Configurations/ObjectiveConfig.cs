using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class ObjectiveConfig : IEntityTypeConfiguration<Objective>
    {
        public void Configure(EntityTypeBuilder<Objective> builder)
        {
            builder.HasKey(ci => ci.Id);

           
     

        }

    }
}
