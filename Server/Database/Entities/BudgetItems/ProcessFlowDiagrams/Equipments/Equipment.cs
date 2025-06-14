using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Equipments
{
    public class Equipment : BudgetItem
    {

        public override string Letter { get; set; } = "D";

       
        [NotMapped]
        public override int OrderList => 4;

        public static Equipment Create(Guid ProjectId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProjectId = ProjectId,


            };
        }
        protected override double _BudgetUSD  => EquipmentItems.Count == 0 ? _SettedBudgetUSD : EquipmentItems.Sum(x => x.BudgetUSD);
        public ICollection<BasicEquipmentItem> EquipmentItems { get; set; } = new List<BasicEquipmentItem>();
    }

}
