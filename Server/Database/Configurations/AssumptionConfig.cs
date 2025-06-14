using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class AssumptionConfig : IEntityTypeConfiguration<Assumption>
    {
        public void Configure(EntityTypeBuilder<Assumption> builder)
        {
            builder.HasKey(ci => ci.Id);

          

        }

    }
}
