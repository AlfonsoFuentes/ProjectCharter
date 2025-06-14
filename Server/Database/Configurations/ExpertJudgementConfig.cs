using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Database.Entities.ProjectManagements;

namespace Server.Database.Configurations
{
    internal class ExpertJudgementConfig : IEntityTypeConfiguration<ExpertJudgement>
    {
        public void Configure(EntityTypeBuilder<ExpertJudgement> builder)
        {
            builder.HasOne(c => c.Expert)
           .WithMany(t => t.Judgements)
           .HasForeignKey(x => x.ExpertId)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
