using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class BenefitConfig : IEntityTypeConfiguration<Bennefit>
    {
        public void Configure(EntityTypeBuilder<Bennefit> builder)
        {
            builder.HasKey(ci => ci.Id);



        }

    }
}
