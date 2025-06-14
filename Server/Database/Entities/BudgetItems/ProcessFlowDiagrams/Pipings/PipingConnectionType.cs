using Server.Database.Contracts;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class PipingConnectionType : AuditableEntity<Guid>, ITenantCommon
    {
        [NotMapped]
        public string Name => $"{NominalDiameter} {EndType}";
        public string EndType { get; set; } = string.Empty;
        public string NominalDiameter { get; set; } = string.Empty;
        public string WeldType { get; set; } = string.Empty;
        public double OuterDiameter { get; set; }
        public double Thickness { get; set; }
        public string OuterDiameterUnit { get; set; } = string.Empty;
        public string ThicknessUnit { get; set; } = string.Empty;

        public PipingAccesory PipingAccesory { get; set; } = null!;
        public Guid PipingAccesoryId { get; set; }
        public AccesoryConnectionSide AccesoryConnectionSide { get; set; }
    }

}
