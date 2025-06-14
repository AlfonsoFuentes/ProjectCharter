using Server.Database.Contracts;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Nozzles
{
    public class Nozzle : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public BasicEngineeringItem BasicEngineeringItem { get; set; } = null!;
        public Guid BasicEngineeringItemId { get; set; }
        //public EngineeringItem EngineeringItem { get; set; } = null!;
        //public Guid EngineeringItemId { get; set; }
        //public EngineeringItem? ItemConnected { get; set; } = null!;
        //public Guid? ItemConnectedId { get; set; }
        public BasicEngineeringItem? BasicEngineeringItemConnected { get; set; } = null!;
        public Guid? BasicEngineeringItemConnectedId { get; set; }
        [NotMapped]
        public string Name => $"N{Order}";
        public int ConnectionType { get; set; }
        public int NominalDiameter { get; set; } 
        public int NozzleType { get; set; } 
  
        public double OuterDiameter { get; set; }
        public double InnerDiameter { get; set; }
        public double Thickness { get; set; }
        public double Height { get; set; }
        public string OuterDiameterUnit { get; set; } = string.Empty;
        public string ThicknessUnit { get; set; } = string.Empty;
        public string InnerDiameterUnit { get; set; } = string.Empty;
        public string HeightUnit { get; set; } = string.Empty;


        public static Nozzle Create(Guid EngineeringItemId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                BasicEngineeringItemId = EngineeringItemId,
            };
        }

    }

}
