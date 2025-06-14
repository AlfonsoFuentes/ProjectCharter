using Server.Database.Contracts;
using Shared.Models.BudgetItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.Commons
{
    public class Alteration : BudgetItem
    {
        public override string Letter { get; set; } = "A";
        public string CostCenter { get; set; } = string.Empty;
        public double UnitaryCost { get; set; }
        public double Quantity { get; set; }
        [NotMapped]
        public override int OrderList => 1;

        public static Alteration Create(Guid ProjectId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProjectId = ProjectId,
              
                
            };
        }


    }

}
