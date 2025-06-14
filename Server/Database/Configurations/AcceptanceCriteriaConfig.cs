using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class AcceptanceCriteriaConfig : IEntityTypeConfiguration<AcceptanceCriteria>
    {
        public void Configure(EntityTypeBuilder<AcceptanceCriteria> builder)
        {
            builder.HasKey(ci => ci.Id);



        }

    }
}
