using Server.Database.Contracts;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class PipingAccesoryCodeBrand : AuditableEntity<Guid>, ITenantCommon
    {
        public PipingCategory PipingCategory { get; set; } = null!;
        public Guid PipingCategoryId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;


    }

}
