namespace Shared.Models.BudgetItems.Exports
{
    public record BudgetItemWithPurchaseordersExport(string Nomenclatore, string Name, double BudgetUSD,double ActualUSD,double CommitmentUSD,double PotentialUSD,double ToCommitUSD);

}