using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.InitialLevelBigWips;
using Server.EndPoint.FinishingLines.ProductionLineAssignments;
using Server.EndPoint.FinishingLines.ProductionScheduleItems;
using Shared.Models.FinishingLines.ProductionPlans;

namespace Server.EndPoint.FinishingLines.ProductionPlans
{
    public static class CreateUpdateProductionPlanEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.CreateUpdate, async (ProductionPlanResponse Data, IRepository Repository) =>
                {
                    ProductionPlan? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = ProductionPlan.Create();

                        await Repository.AddAsync(row);
                        foreach (var item in Data.LineAssignments)
                        {
                            ProductionLineAssignment? lineAssignment = null!;
                            lineAssignment = ProductionLineAssignment.Create(row.Id, item.ProductionLine.Id);
                            await Repository.AddAsync(lineAssignment);
                            item.Map(lineAssignment);
                            foreach (var schedule in item.OrderedScheduleItems)
                            {
                                var productionScheduleItem = ProductionScheduleItem.Create(lineAssignment.Id, schedule.Sku!.Id, schedule.Order);
                                await Repository.AddAsync(productionScheduleItem);
                                schedule.Map(productionScheduleItem);
                            }
                        }
                        foreach (var item in Data.InitialLevelBigWips)
                        {
                            InitialLevelBigWip initialLevelBigWip = InitialLevelBigWip.Create(row.Id, item.BigWipTank.Id);
                            item.Map(initialLevelBigWip);
                            await Repository.AddAsync(initialLevelBigWip);
                        }
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<ProductionPlan>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }



                    Data.Map(row);
                    List<string> cache = [.. StaticClass.ProductionPlans.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static ProductionPlan Map(this ProductionPlanResponse request, ProductionPlan row)
        {
            row.Name = request.Name;

            return row;
        }

    }
    public static class DeleteProductionPlanEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.Delete, async (DeleteProductionPlanRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<ProductionPlan>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.ProductionPlans.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductionPlanEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.DeleteGroup, async (DeleteGroupProductionPlanRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<ProductionPlan>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.ProductionPlans.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductionPlanEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.GetAll, async (ProductionPlanGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionPlan>, IIncludableQueryable<ProductionPlan, object>> includes = x => x
                      .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionLine).ThenInclude(x => x.LineSpeeds)
                       .ThenInclude(x => x.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)

                       .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionScheduleItems).ThenInclude(x => x.SKU)
                       .ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                          .Include(x => x.InitialLevelBigWips).ThenInclude(x => x.BIGWIPTank!).ThenInclude(x => x.Backbone)

                       ;

                    string CacheKey = StaticClass.ProductionPlans.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<ProductionPlanResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ProductionPlans.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductionPlanResponseList response = new ProductionPlanResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductionPlanResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductionPlanByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.GetById, async (GetProductionPlanByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionPlan>, IIncludableQueryable<ProductionPlan, object>> includes = x => x
                     .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionLine).ThenInclude(x => x.LineSpeeds)
                       .ThenInclude(x => x.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)

                       .Include(p => p.LineAssignments).ThenInclude(x => x.ProductionScheduleItems).ThenInclude(x => x.SKU)
                       .ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)

                       .Include(x => x.InitialLevelBigWips).ThenInclude(x => x.BIGWIPTank!).ThenInclude(x => x.Backbone)
                      ;
                    Expression<Func<ProductionPlan, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ProductionPlans.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: includes);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static ProductionPlanResponse Map(this ProductionPlan row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,

                LineAssignments = row.LineAssignments.Select(x => x.Map()).ToList(),
                InitialLevelBigWips = row.InitialLevelBigWips.Select(x => x.Map()).ToList()
            };
        }

    }
    public static class ValidateProductionPlansNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionPlans.EndPoint.Validate, async (ValidateProductionPlanNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<ProductionPlan, bool>> CriteriaId = null!;
                    Func<ProductionPlan, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.ProductionPlans.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
