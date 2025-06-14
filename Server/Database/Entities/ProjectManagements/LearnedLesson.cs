using Server.Database.Contracts;

namespace Server.Database.Entities.ProjectManagements
{
    public class LearnedLesson : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public static LearnedLesson Create(Guid ProjectId,  int Order)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProjectId = ProjectId,
                Order = Order,
               
               
            };
        }

       
        
    }
}
