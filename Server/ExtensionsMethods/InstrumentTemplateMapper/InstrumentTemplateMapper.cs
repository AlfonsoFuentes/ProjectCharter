using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.Templates.Instruments.Responses;

namespace Server.ExtensionsMethods.InstrumentTemplateMapper
{
    public static class InstrumentTemplateMapper
    {
      
       
        public static Instrument Map(this InstrumentResponse request, Instrument row)
        {
            row.Name = request.Name;
     
            row.BudgetUSD = request.BudgetUSD;
      
            return row;
        }
        public static InstrumentResponse Map(this Instrument row)
        {
            InstrumentResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                Items = row.InstrumentItems == null || row.InstrumentItems.Count == 0 ? new() : row.InstrumentItems.Select(x => x.Map()).ToList(),
                BudgetUSD = row.InstrumentItems == null || row.InstrumentItems.Count == 0 ? row.BudgetUSD : row.InstrumentItems.Sum(x => x.BudgetUSD),
                //HasSubItems = row.InstrumentItems != null && row.InstrumentItems.Count > 0,
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
