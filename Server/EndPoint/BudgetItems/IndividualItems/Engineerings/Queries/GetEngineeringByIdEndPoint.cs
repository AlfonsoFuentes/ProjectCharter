using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Engineerings.Responses;

namespace Server.EndPoint.BudgetItems.IndividualItems.Engineerings.Queries
{
    public static class GetEngineeringByIdEndPoint
    {

        public static EngineeringResponse Map(this Engineering row)
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
