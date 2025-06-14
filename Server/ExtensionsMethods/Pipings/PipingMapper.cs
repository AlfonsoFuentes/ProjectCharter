using Server.EndPoint.Brands.Queries;
using Server.EndPoint.EngineeringFluidCodes.Queries;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace Server.ExtensionsMethods.Pipings
{
    public static class PipingMapper
    {


        public static Pipe Map(this PipeResponse request, Pipe row)
        {
            row.Name = request.Name;
            row.BudgetUSD = request.BudgetUSD;


            return row;
        }
        public static PipeResponse Map(this Pipe row)
        {
            PipeResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
               
                Items = row.PipeItems == null || row.PipeItems.Count == 0 ? new() : row.PipeItems.Select(x => x.Map()).ToList(),
        
                BudgetUSD = row.PipeItems == null || row.PipeItems.Count == 0 ? row.BudgetUSD : row.PipeItems.Sum(x => x.BudgetUSD),
                //HasSubItems = row.PipeItems != null && row.PipeItems.Count > 0,
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
