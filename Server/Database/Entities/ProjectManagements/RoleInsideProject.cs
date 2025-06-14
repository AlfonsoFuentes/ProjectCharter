using Server.Database.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.ProjectManagements
{
    public class RoleInsideProject : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        [ForeignKey("RoleInsideProjectId")]
        public List<StakeHolder> StakeHolders { get; set; } = new();
        public Guid ProjectId { get; set; }

        public static RoleInsideProject Create(Guid _projectId, string Name)
        {
            return new()
            {
                Id = Guid.NewGuid(),

                ProjectId = _projectId,
                Name = Name
            };
        }
    }
}
