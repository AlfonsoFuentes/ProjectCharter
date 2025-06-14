using Server.EndPoint.Brands.Queries;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;

namespace Server.ExtensionsMethods.ValveTemplateMappers
{
    public static class ValveTemplateMapper
    {

        public static Valve Map(this ValveResponse request, Valve row)
        {
            row.Name = request.Name;
          
            row.BudgetUSD = request.BudgetUSD;
         
            return row;
        }

        public static ValveResponse Map(this Valve row)
        {
            ValveResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                Items = row.ValveItems == null || row.ValveItems.Count == 0 ? new() : row.ValveItems.Select(x => x.Map()).ToList(),
                BudgetUSD = row.ValveItems == null || row.ValveItems.Count == 0 ? row.BudgetUSD : row.ValveItems.Sum(x => x.BudgetUSD),
                //HasSubItems = row.ValveItems != null && row.ValveItems.Count > 0,
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,

                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks == null ? new() : row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
            };

            return result;
        }
    }
}
