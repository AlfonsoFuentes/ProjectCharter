using Server.Database.Contracts;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class IsometricItem : AuditableEntity<Guid>, ITenantEntity
    {

        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        //public Pipe Isometric { get; set; } = null!;
        //public Guid IsometricId { get; set; }
        public PipingAccesory? PipingAccesory { get; set; } = null!;
        public Guid? PipingAccesoryId { get; set; }

        public BasicPipeItem BasicPipeItem { get; set; } = null!;
        public Guid BasicPipeItemId { get; set; }

    }

}
