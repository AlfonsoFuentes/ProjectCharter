namespace Shared.FinshingLines.DPSimulation.Mixers
{
    public class NewScheduledBatch
    {
        public MixerContext MixerContext { get; set; } = null!;
        
        public BackBoneConfiguration BackBone { get; set; } = null!;
        public double BatchTime { get; set; }
        public double BatchSize { get; set; }
       
        public double StartTimeMinute { get; set; }
        public double EndTimeMinute => StartTimeMinute + BatchTime;
        public string MixerName => MixerContext == null ? string.Empty : MixerContext.Name;
        public override string ToString()
        {
            return $"{MixerName} - {BackBone.Name} - {BatchSize} kg - {BatchTime}min";
        }
    }
    
    public class NewSKU
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductConfiguration> ProductBackBones { get; set; } = new();
        public double TimeToChangeFormat { get; set; } = 0;
        public List<Guid> BackboneIds => ProductBackBones.Select(x => x.BackBone.Id).ToList();
        public override string ToString()
        {
            return $"{Name} - {string.Join(", ", ProductBackBones.Select(x => x.ToString()))}";
        }

    }
    public class NewScheduledSKU
    {
        public Guid Id { get; set; }
        public ProductionLineConfiguration Line { get; set; } = null!;
        public double MassPlanned { get; set; }
        public double MassFlow { get; set; }
        public NewSKU SKU { get; set; } = null!;
        public int Order { get; set; }
        public double StartTimeMinute { get; set; }
        public double EndTimeMinute => StartTimeMinute + EstimatedRunTime;
        public double EstimatedRunTime { get; set; }
        public double GetMassPlannedByBackBone(Guid _backboneId)
        {
            double result = 0;
            if (SKU == null) return 0;
            var productbackbone = SKU.ProductBackBones.FirstOrDefault(x => x.BackBone.Id == _backboneId);
            if (productbackbone == null) return 0;
            result = MassPlanned * productbackbone.Percentage / 100;
            return result;
        }
        public override string ToString()
        {
            return $"{SKU.Name} order: {Order} - {MassPlanned}kg - {MassFlow}kg/min - {Math.Round(EstimatedRunTime)}min";
        }

    }
}
