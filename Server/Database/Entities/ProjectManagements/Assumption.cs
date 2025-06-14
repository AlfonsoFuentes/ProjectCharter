using Server.Database.Contracts;

namespace Server.Database.Entities.ProjectManagements
{
    public class Assumption : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;

        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }



        public string Name { set; get; } = string.Empty;

        public static Assumption Create(Guid ProjectId,  int Order)
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
