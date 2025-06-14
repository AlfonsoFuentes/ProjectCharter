using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class BackGroundConfig : IEntityTypeConfiguration<BackGround>
    {
        public void Configure(EntityTypeBuilder<BackGround> builder)
        {
            builder.HasKey(ci => ci.Id);



        }

    }
}
