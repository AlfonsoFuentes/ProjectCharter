namespace Server.Database.FinishlinLines
{


    public class SKU : AuditableEntity<Guid>, ITenantCommon
    {

        public string Name { get; set; } = string.Empty;
        public double MassPerEA { get; set; }
        public string MassPerEAUnit { get; set; } = string.Empty;

        public double VolumePerEA { get; set; }
        public string VolumePerEAUnit { get; set; } = string.Empty;

        [ForeignKey("SkuId")]
        public ICollection<ProductionScheduleItem> ProductionScheduleItems { get; set; } = new List<ProductionScheduleItem>();


        public Product? Product { get; set; } = null!;
        public Guid? ProductId { get; set; }
        [ForeignKey("SkuId")]
        public ICollection<LineSpeed> LineSpeeds { get; set; } = new List<LineSpeed>();
        public static SKU Create()
        {
            return new SKU
            {
                Id = Guid.NewGuid(),

            };
        }
    }
    public class Backbone : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;


        //// Relaciones
        //[ForeignKey("SkuId")]
        //public ICollection<SKUComponent> SkuComponents { get; set; } = new List<SKUComponent>();

        [ForeignKey("BackboneId")]
        public ICollection<MixerBackbone> MixerBackbones { get; set; } = new List<MixerBackbone>();

        [ForeignKey("BackboneId")]
        public ICollection<BIGWIPTank> BIGWIPTanks { get; set; } = new List<BIGWIPTank>();

        [ForeignKey("BackboneId")]
        public ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();

        public static Backbone Create()
        {
            return new Backbone
            {
                Id = Guid.NewGuid(),

            };
        }
    }

    public class ProductComponent : AuditableEntity<Guid>, ITenantCommon
    {

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;


        public Guid? BackboneId { get; set; }
        public Backbone? Backbone { get; set; } = null!;

        public double Percentage { get; set; }
        public static ProductComponent Create(Guid BackboneId, Guid ProductId)
        {
            return new ProductComponent
            {
                Id = Guid.NewGuid(),
                BackboneId = BackboneId,
                ProductId = ProductId,

            };
        }
    }
    public class Product : AuditableEntity<Guid>, ITenantCommon
    {
        [ForeignKey("ProductId")]
        public ICollection<SKU> SKUs { get; set; } = new List<SKU>();
        public string Name { get; set; } = string.Empty;

        public ICollection<ProductComponent> Components { get; set; } = new List<ProductComponent>();

        public static Product Create()
        {
            return new Product
            {
                Id = Guid.NewGuid(),


            };
        }
    }
    public class Mixer : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;


        public double CleaningTime { get; set; }
        public string CleaningTimeUnit { get; set; } = string.Empty;

        // Relación
        [ForeignKey("MixerId")]
        public ICollection<MixerBackbone> Capabilities { get; set; } = new List<MixerBackbone>();
        public static Mixer Create()
        {
            return new Mixer
            {
                Id = Guid.NewGuid(),

            };
        }
    }
    public class MixerBackbone : AuditableEntity<Guid>, ITenantCommon
    {

        public Guid MixerId { get; set; }
        public Mixer Mixer { get; set; } = null!;


        public Guid BackboneId { get; set; }
        public Backbone Backbone { get; set; } = null!;
        public double BatchTime { get; set; }
        public string BatchTimeUnit { get; set; } = string.Empty;
        public double Capacity { get; set; }
        public string CapacityUnit { get; set; } = string.Empty;
        public static MixerBackbone Create(Guid MixerId, Guid BackboneId)
        {
            return new MixerBackbone
            {
                Id = Guid.NewGuid(),
                MixerId = MixerId,
                BackboneId = BackboneId,

            };
        }
    }

    public class ProductionLine : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;

        // Relaciones
        [ForeignKey("LineId")]
        public ICollection<LineSpeed> LineSpeeds { get; set; } = new List<LineSpeed>();

        [ForeignKey("LineId")]
        public ICollection<WIPTankLine> WIPTanks { get; set; } = new List<WIPTankLine>();

        [ForeignKey("LineId")]
        public ICollection<ProductionLineAssignment> ProductionLineAssignments { get; set; } = new List<ProductionLineAssignment>();
        public double CleaningTime { get; set; }
        public string CleaningTimeUnit { get; set; } = string.Empty;
        public double FormatChangeTime { get; set; }
        public string FormatChangeTimeUnit { get; set; } = string.Empty;
        public static ProductionLine Create()
        {
            return new ProductionLine
            {
                Id = Guid.NewGuid(),

            };
        }
    }
    public class LineSpeed : AuditableEntity<Guid>, ITenantCommon
    {

        public Guid LineId { get; set; }
        public ProductionLine ProductionLine { get; set; } = null!;


        public Guid SkuId { get; set; }
        public SKU Sku { get; set; } = null!;

        public double PercentageAU { get; set; }
        public double MaxSpeed { get; set; }
        public string MaxSpeedUnit { get; set; } = string.Empty;
        public static LineSpeed Create(Guid LineId, Guid SkuId)
        {
            return new LineSpeed
            {
                Id = Guid.NewGuid(),
                LineId = LineId,
                SkuId = SkuId,
            };
        }
    }

    public class ProductionPlan : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;

       
        public List<InitialLevelBigWip> InitialLevelBigWips { get; set; } = new();
        // Relaciones
        [ForeignKey("PlanId")]
        public ICollection<ProductionLineAssignment> LineAssignments { get; set; } = new List<ProductionLineAssignment>();
        public static ProductionPlan Create()
        {
            return new ProductionPlan
            {
                Id = Guid.NewGuid(),

            };
        }
    }
    public class ProductionLineAssignment : AuditableEntity<Guid>, ITenantCommon
    {

        public Guid PlanId { get; set; }
        public ProductionPlan ProductionPlan { get; set; } = null!;


        public Guid LineId { get; set; }
        public ProductionLine ProductionLine { get; set; } = null!;
        public List<InitialLevelWip> InitialLevelWips { get; set; } = new();
        // Relación
        [ForeignKey("ProductionLineAssignmentId")]
        public ICollection<ProductionScheduleItem> ProductionScheduleItems { get; set; } = new List<ProductionScheduleItem>();
        public static ProductionLineAssignment Create(Guid PlanId, Guid LineId)
        {
            return new ProductionLineAssignment
            {
                Id = Guid.NewGuid(),
                PlanId = PlanId,
                LineId = LineId,

            };
        }
    }

    public class ProductionScheduleItem : AuditableEntity<Guid>, ITenantCommon
    {

        public Guid ProductionLineAssignmentId { get; set; }
        public ProductionLineAssignment ProductionLineAssignment { get; set; } = null!;


        public Guid SkuId { get; set; }
        public SKU SKU { get; set; } = null!;

        public double PlannedMass { get; set; }
        public string PlannedMassUnit { get; set; } = string.Empty;



        public static ProductionScheduleItem Create(Guid ProductionLineAssignmentId, Guid SkuId, int order)
        {
            return new ProductionScheduleItem
            {
                Id = Guid.NewGuid(),
                ProductionLineAssignmentId = ProductionLineAssignmentId,
                SkuId = SkuId,
                Order = order
            };
        }
    }

    public class InitialLevelWip : AuditableEntity<Guid>, ITenantCommon
    {
        public Guid WIPTankLineId { get; set; }
        public WIPTankLine? WIPTankLine { get; set; }
        public ProductionLineAssignment ProductionLineAssignment { get; set; } = null!;
        public Guid ProductionLineAssignmentId { get; set; }
        public double InitialLevel { get; set; }
        public string InitialLevelUnit { get; set; } = string.Empty;
        public static InitialLevelWip Create(Guid ProductionLineAssignmentId, Guid WIPTankLineId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProductionLineAssignmentId = ProductionLineAssignmentId,
                WIPTankLineId = WIPTankLineId
            };
        }
    }
    public class InitialLevelBigWip : AuditableEntity<Guid>, ITenantCommon
    {
        public Guid BIGWIPTankId { get; set; }
        public BIGWIPTank? BIGWIPTank { get; set; }
        public ProductionPlan ProductionPlan { get; set; } = null!;
        public Guid ProductionPlanId { get; set; }
        public double InitialLevel { get; set; }
        public string InitialLevelUnit { get; set; } = string.Empty;
        public static InitialLevelBigWip Create(Guid ProductionPlanId, Guid BigWipId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProductionPlanId = ProductionPlanId,
                BIGWIPTankId = BigWipId
            };
        }
    }
    public class BIGWIPTank : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        public Guid BackboneId { get; set; }
        public Backbone Backbone { get; set; } = null!;

        public double CleaningTime { get; set; }
        public string CleaningTimeUnit { get; set; } = string.Empty;
        public double Capacity { get; set; }
        public string CapacityUnit { get; set; } = string.Empty;
        public double InletMassFlow { get; set; }
        public string InletMassFlowUnit { get; set; } = string.Empty;
        public double OutletMassFlow { get; set; }
        public string OutletMassFlowUnit { get; set; } = string.Empty;
        public double MinimumTransferLevelKgPercentage { get; set; }

        public static BIGWIPTank Create(Guid BackboneId)
        {
            return new BIGWIPTank
            {
                Id = Guid.NewGuid(),
                BackboneId = BackboneId,
            };
        }
        [ForeignKey("BIGWIPTankId")]
        public ICollection<InitialLevelBigWip> InitialLevelBigWips { get; set; } = new List<InitialLevelBigWip>();

    }
    public class WIPTankLine : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        public Guid LineId { get; set; }
        public ProductionLine Line { get; set; } = null!;
        public double MinimumLevelPercentage { get; set; }
        public double Capacity { get; set; }
        public string CapacityUnit { get; set; } = string.Empty;
        public double CleaningTime { get; set; }
        public string CleaningTimeUnit { get; set; } = string.Empty;
        [ForeignKey("WIPTankLineId")]
        public ICollection<InitialLevelWip> InitialLevelWips { get; set; } = new List<InitialLevelWip>();
        public static WIPTankLine Create(Guid LineId)
        {
            return new WIPTankLine
            {
                Id = Guid.NewGuid(),
                LineId = LineId,

            };
        }


    }
}
