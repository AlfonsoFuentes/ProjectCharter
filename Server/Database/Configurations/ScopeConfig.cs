using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class ScopeConfig : IEntityTypeConfiguration<Scope>
    {
        public void Configure(EntityTypeBuilder<Scope> builder)
        {
            builder.HasKey(ci => ci.Id);

      
        }

    }
}
