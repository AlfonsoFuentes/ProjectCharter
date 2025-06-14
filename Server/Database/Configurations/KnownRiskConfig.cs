using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class KnownRiskConfig : IEntityTypeConfiguration<KnownRisk>
    {
        public void Configure(EntityTypeBuilder<KnownRisk> builder)
        {
            builder.HasKey(ci => ci.Id);



        }

    }
}
