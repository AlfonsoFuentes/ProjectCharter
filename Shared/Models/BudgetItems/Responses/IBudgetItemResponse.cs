using Shared.Enums.BudgetItemTypes;
using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.BudgetItems.Responses
{
    public interface IBudgetItemResponse : IResponse
    {
      
        string Nomenclatore { get; set; }
        string NomenclatoreName { get; }
        double BudgetUSD { get; }


        bool IsAlteration { get; set; }
        bool IsTaxes { get; set; }

    }
    public interface IBudgetItemWithPurchaseOrderResponse : IBudgetItemResponse
    {

       
        double ActualUSD { get; set; }
        double CommitmentUSD { get; set; }
        double PotentialUSD { get; set; }
        double AssignedUSD { get; }
        double ToCommitUSD { get; }
    
        
        
    }
}
