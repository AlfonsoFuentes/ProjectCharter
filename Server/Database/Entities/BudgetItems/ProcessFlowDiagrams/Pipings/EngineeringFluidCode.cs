using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class EngineeringFluidCode : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        //[ForeignKey("FluidCodeId")]
        //public ICollection<Pipe> Isometrics { get; set; } = new List<Pipe>();
        [ForeignKey("BasicFluidCodeId")]
        public ICollection<BasicPipeItem> BasicPipeItems { get; set; } = new List<BasicPipeItem>();
        
        public static EngineeringFluidCode Create()
        {
            return new EngineeringFluidCode()
            {
                Id = Guid.NewGuid(),
            };
        }
    }

}
