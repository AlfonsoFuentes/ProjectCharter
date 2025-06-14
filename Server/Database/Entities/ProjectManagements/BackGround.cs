using Server.Database.Contracts;

namespace Server.Database.Entities.ProjectManagements
{
    public class BackGround : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;

        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public static BackGround Create(Guid ProjectId, int Order)
        {
            return new BackGround()
            {
                Id = Guid.NewGuid(),
                ProjectId = ProjectId,
                Order = Order,

            };
        }

    }
}
