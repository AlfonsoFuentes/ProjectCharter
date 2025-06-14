using Server.EndPoint.Brands.Queries;
using Server.EndPoint.MonitoringTasks.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
namespace Server.ExtensionsMethods.EquipmentTemplateMapper
{
    public static class EquipmentTemplateMapper
    {
        public static EquipmentResponse Map(this Equipment row)
        {

            return new()
            {
                Id = row.Id,
                Name = row.Name,
                OrderList = row.OrderList,
                ProjectId = row.ProjectId,
                Nomenclatore = row.Nomenclatore,
                
                Items = row.EquipmentItems == null || row.EquipmentItems.Count == 0 ? new() : row.EquipmentItems.Select(x => x.Map()).ToList(),
                BudgetUSD = row.EquipmentItems == null || row.EquipmentItems.Count == 0 ? row.BudgetUSD : row.EquipmentItems.Sum(x => x.BudgetUSD),
                ActualUSD = row.ActualUSD,
                CommitmentUSD = row.CommitmentUSD,
                PotentialUSD = row.PotentialUSD,
                PurchaseOrders = row.PurchaseOrderItems == null ? new() : row.PurchaseOrderItems.Select(x => x.PurchaseOrder).Select(x => x.Map()).ToList(),
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks == null ? new() : row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
            };


        }
        public static Equipment Map(this EquipmentResponse request, Equipment row)
        {
            row.Name = request.Name;

            row.BudgetUSD = request.BudgetUSD;

            return row;
        }


    }
}
