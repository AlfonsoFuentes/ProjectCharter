using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Contingencys.Responses;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Records;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Contingencys.Queries
{
    public static class GetContingencyByIdEndPoint
    {
     
        public static ContingencyResponse Map(this Contingency row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                Percentage = row.Percentage,
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,
                BudgetUSD = row.BudgetUSD,
                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
            };
        }


    }
}
