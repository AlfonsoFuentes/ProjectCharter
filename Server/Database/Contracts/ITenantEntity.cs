using Server.Database.Entities.BudgetItems;
using Shared.Models.BudgetItems;

namespace Server.Database.Contracts
{
    public interface ITenantEntity
    {
        string TenantId { get; set; }

    }
   
}
