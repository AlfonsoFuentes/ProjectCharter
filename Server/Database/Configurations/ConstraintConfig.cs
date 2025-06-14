using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class ConstraintConfig : IEntityTypeConfiguration<Constrainst>
    {
        public void Configure(EntityTypeBuilder<Constrainst> builder)
        {
            builder.HasKey(ci => ci.Id);

        

        }

    }
}
