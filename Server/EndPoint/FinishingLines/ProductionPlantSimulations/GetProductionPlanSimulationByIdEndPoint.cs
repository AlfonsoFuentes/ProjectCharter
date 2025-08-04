
using Server.Database.FinishlinLines;
using Shared.FinshingLines;
using Shared.Models.FinishingLines.ProductionPlanSimulations;

namespace Server.EndPoint.FinishingLines.ProductionPlans
{
    public static class GetProductionPlanSimulationByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlanSimulations.EndPoint.GetById, async (GetProductionPlanSimulationByIdRequest request, IQueryRepository Repository) =>
                {
                    ReadSimulationFromDatabase database = new();
                    await database.ReadBackBones(Repository);
                    await database.ReadProducts(Repository);
                    await database.ReadMixers(Repository);
                    await database.ReadBigWips(Repository);
                    await database.ReadProductionPlan(request.Id, Repository);
                    return Result.Success(database);

                });
            }

        }
        public static async Task ReadBackBones(this ReadSimulationFromDatabase database, IQueryRepository Repository)
        {
            string CacheKey = StaticClass.Backbones.Cache.GetAll;
            var rows = await Repository.GetAllAsync<Backbone>(CacheKey);

            database.BackBones = rows.Select(x => x.MapBackbone()).ToList();
        }
        public static BackBoneConfiguration MapBackbone(this Backbone row)
        {
            return new BackBoneConfiguration()
            {
                Id = row.Id,
                Name = row.Name,
            };
        }
        public static async Task ReadProducts(this ReadSimulationFromDatabase database, IQueryRepository Repository)
        {
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes = x => x
                        .Include(p => p.Components).ThenInclude(x => x.Backbone!)
                         ;

            string CacheKey = StaticClass.Products.Cache.GetAll;
            var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

            database.Products = rows.Select(x => x.MapProduct()).ToList();
        }
        public static ProductBaseConfiguration MapProduct(this Product row)
        {
            return new ProductBaseConfiguration()
            {
                ProductId = row.Id,
                ProductName = row.Name,
                Components = row.Components.Select(c => new ProductConfiguration()
                {
                    Percentage = c.Percentage,
                    BackBone = c.Backbone == null ? null! : c.Backbone.MapBackbone(),
                }).ToList()

            };
        }

        public static async Task ReadMixers(this ReadSimulationFromDatabase database, IQueryRepository Repository)
        {
            Func<IQueryable<Mixer>, IIncludableQueryable<Mixer, object>> includes = x => x
                    .Include(p => p.Capabilities).ThenInclude(x => x.Backbone)
                     ;
            string CacheKey = StaticClass.Mixers.Cache.GetAll;
            var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);
            database.Mixers = rows.Select(x => x.MapMixer()).ToList();
        }
        public static MixerConfiguration MapMixer(this Mixer row)
        {
            return new MixerConfiguration()
            {
                MixerId = row.Id,
                Name = row.Name,
                CleaningTime = new Shared.FinshingLines.Time(row.CleaningTime, Shared.FinshingLines.TimeUnits.Hour),
                Capabilities = row.Capabilities.Select(c => new BackboneCapability()
                {
                    BackBone = c.Backbone == null ? null! : c.Backbone.MapBackbone(),
                    BatchTime = new Shared.FinshingLines.Time(c.BatchTime, Shared.FinshingLines.TimeUnits.Minute),
                    Capacity = new Shared.FinshingLines.Mass(c.Capacity, Shared.FinshingLines.MassUnits.KiloGram),

                }).ToList()

            };
        }
        public static async Task ReadBigWips(this ReadSimulationFromDatabase database, IQueryRepository Repository)
        {
            string CacheKey = StaticClass.BIGWIPTanks.Cache.GetAll;
            Func<IQueryable<BIGWIPTank>, IIncludableQueryable<BIGWIPTank, object>> includes = x => x.Include(y => y.Backbone);
            var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);
            database.BigWIPTanks = rows.Select(x => x.MapBigWip()).ToList();
        }
        public static BigWIPTankConfiguration MapBigWip(this BIGWIPTank row)
        {
            return new BigWIPTankConfiguration()
            {
                TankId = row.Id,

                Capacity = new Shared.FinshingLines.Mass(row.Capacity, Shared.FinshingLines.MassUnits.KiloGram),
                BackBone = row.Backbone == null ? null! : row.Backbone.MapBackbone(),
                CleaningTime = new Shared.FinshingLines.Time(row.CleaningTime, Shared.FinshingLines.TimeUnits.Minute),
                InletMassFlow = new Shared.FinshingLines.MassFlow(row.InletMassFlow, Shared.FinshingLines.MassFlowUnits.Kg_min),
                OutletMassFlow = new Shared.FinshingLines.MassFlow(row.OutletMassFlow, Shared.FinshingLines.MassFlowUnits.Kg_min),
                MinimumTransferLevelKgPercentage = row.MinimumTransferLevelKgPercentage,


            };
        }
        public static async Task ReadProductionPlan(this ReadSimulationFromDatabase database, Guid Id, IQueryRepository Repository)
        {
            Func<IQueryable<ProductionPlan>, IIncludableQueryable<ProductionPlan, object>> includes = x => x
                    .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionLine).ThenInclude(x => x.LineSpeeds)
                      .ThenInclude(x => x.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                      .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionLine).ThenInclude(x => x.WIPTanks)
                      .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionScheduleItems).ThenInclude(x => x.SKU)
                      .ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)

                     ;
            Expression<Func<ProductionPlan, bool>> Criteria = x => x.Id == Id;

            string CacheKey = StaticClass.ProductionPlans.Cache.GetById(Id);
            var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: includes);
            if (row == null)
            {
                if (row == null) { return; }
            }
            database.MapProductionPlan(row);
        }
        public static void MapProductionPlan(this ReadSimulationFromDatabase database, ProductionPlan row)
        {
            foreach (var assigment in row.LineAssignments)
            {
                var productionLine = assigment.ProductionLine.MapProductionLine();
                database.ProductionLines.Add(productionLine);
                int order = 0;
                foreach (var schedule in assigment.ProductionScheduleItems)
                {
                    order++;
                    var sku = schedule.SKU;
                    var linespeed = assigment.ProductionLine.LineSpeeds.FirstOrDefault(x => x.SkuId == schedule.SKU.Id);
                    double realvelocity = linespeed == null ? 0 : linespeed.MaxSpeed * linespeed.PercentageAU / 100;
                    productionLine.MassPlannedPerLineProducts.Add(new MassPlannedPerLineProductBaseConfiguration()
                    {
                        Id = schedule.Id,
                        Order = order,
                        MassPlanned = new Shared.FinshingLines.Mass(schedule.PlannedMass, Shared.FinshingLines.MassUnits.KiloGram),
                        Product = schedule.SKU.Product!.MapProduct(),
                        FormatMass = new Shared.FinshingLines.Mass(schedule.SKU.MassPerEA, Shared.FinshingLines.MassUnits.Gram),
                        TimeToChangeFormat = new Shared.FinshingLines.Time(assigment.ProductionLine.FormatChangeTime, Shared.FinshingLines.TimeUnits.Minute),
                        LineVelocity = new Shared.FinshingLines.LineVelocity(realvelocity, Shared.FinshingLines.LineVelocityUnits.EA_min)
                        ,
                    });
                }
            }
        }

        public static ProductionLineConfiguration MapProductionLine(this ProductionLine row)
        {
            return new ProductionLineConfiguration()
            {
                LineId = row.Id,
                Name = row.Name,
                CleaningTime = new Shared.FinshingLines.Time(row.CleaningTime, Shared.FinshingLines.TimeUnits.Minute),
                FormatChangeTime = new Shared.FinshingLines.Time(row.FormatChangeTime, Shared.FinshingLines.TimeUnits.Minute),
                WIPTanks = row.WIPTanks.Select(x => x.MapWipTanLine()).ToList(),


            };
        }
        public static WIPtankForLineConfiguration MapWipTanLine(this WIPTankLine row)
        {
            return new WIPtankForLineConfiguration()
            {
                TankId = row.Id,
                Name = row.Name ?? string.Empty,
                Capacity = new Shared.FinshingLines.Mass(row.Capacity, Shared.FinshingLines.MassUnits.KiloGram),
                CleaningTime = new Shared.FinshingLines.Time(row.CleaningTime, Shared.FinshingLines.TimeUnits.Minute),
                MinimumLevelPercentage = row.MinimumLevelPercentage,

            };
        }
    }
}
