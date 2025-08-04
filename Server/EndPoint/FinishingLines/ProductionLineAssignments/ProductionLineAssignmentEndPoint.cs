using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.ProductionLines;
using Server.EndPoint.FinishingLines.ProductionScheduleItems;
using Server.EndPoint.FinishingLines.WIPTankLines;
using Shared.Models.FinishingLines.InitialLevelWips;
using Shared.Models.FinishingLines.ProductionLineAssignments;

namespace Server.EndPoint.FinishingLines.ProductionLineAssignments
{
    public static class CreateUpdateProductionLineAssignmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLineAssignments.EndPoint.CreateUpdate, async (ProductionLineAssignmentResponse Data, IRepository Repository) =>
                {
                    ProductionLineAssignment? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = ProductionLineAssignment.Create(Data.PlanId, Data.ProductionLine.Id);
                        foreach (var initiallevel in Data.InitialLevelWips)
                        {
                            var rowinitialevel = InitialLevelWip.Create(row.Id, initiallevel.WipTankLine.Id);
                            rowinitialevel.InitialLevel = initiallevel.InitialLevelValue;
                            rowinitialevel.InitialLevelUnit = initiallevel.InitialLevelUnit;
                            await Repository.AddAsync(rowinitialevel);
                        }
                        foreach (var scheduleItem in Data.ScheduleItems)
                        {
                            var rowScheduleItem = ProductionScheduleItem.Create(row.Id, scheduleItem.Sku!.Id, scheduleItem.Order);
                            scheduleItem.Map(rowScheduleItem);
                            await Repository.AddAsync(rowScheduleItem);
                        }
                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Func<IQueryable<ProductionLineAssignment>, IIncludableQueryable<ProductionLineAssignment, object>> includes = x => x
                      .Include(x => x.InitialLevelWips).ThenInclude(x => x.WIPTankLine!);
                        Expression<Func<ProductionLineAssignment, bool>> Criteria = x => x.Id == Data.Id;

                        row = await Repository.GetAsync(Criteria: Criteria, Includes: includes);

                        if (row == null) { return Result.Fail(Data.NotFound); }
                        
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.ProductionLineAssignments.Cache.Key(row.Id), .. StaticClass.ProductionPlans.Cache.Key(row.PlanId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static ProductionLineAssignment Map(this ProductionLineAssignmentResponse request, ProductionLineAssignment row)
        {

            return row;
        }

    }
    public static class DeleteProductionLineAssignmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLineAssignments.EndPoint.Delete, async (DeleteProductionLineAssignmentRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<ProductionLineAssignment>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.ProductionLineAssignments.Cache.Key(row.Id), .. StaticClass.ProductionPlans.Cache.Key(row.PlanId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductionLineAssignmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLineAssignments.EndPoint.DeleteGroup, async (DeleteGroupProductionLineAssignmentRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<ProductionLineAssignment>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.ProductionLineAssignments.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductionLineAssignmentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLineAssignments.EndPoint.GetAll, async (ProductionLineAssignmentGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionLineAssignment>, IIncludableQueryable<ProductionLineAssignment, object>> includes = x => x
                       .Include(p => p.ProductionLine).ThenInclude(x => x.LineSpeeds)
                       .ThenInclude(x => x.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                       .Include(p => p.ProductionScheduleItems).ThenInclude(x => x.SKU).ThenInclude(x => x.Product!)
                       .ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                       .Include(x => x.InitialLevelWips).ThenInclude(x => x.WIPTankLine!)
                        ;


                    string CacheKey = StaticClass.ProductionLineAssignments.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<ProductionLineAssignmentResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ProductionLineAssignments.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductionLineAssignmentResponseList response = new ProductionLineAssignmentResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductionLineAssignmentResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductionLineAssignmentByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLineAssignments.EndPoint.GetById, async (GetProductionLineAssignmentByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionLineAssignment>, IIncludableQueryable<ProductionLineAssignment, object>> includes = x => x
                      .Include(p => p.ProductionLine).ThenInclude(x => x.LineSpeeds)
                      .ThenInclude(x => x.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                      .Include(p => p.ProductionScheduleItems).ThenInclude(x => x.SKU).ThenInclude(x => x.Product!)
                      .ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)
                      .Include(x => x.InitialLevelWips).ThenInclude(x => x.WIPTankLine!)
                       ;

                    Expression<Func<ProductionLineAssignment, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ProductionLineAssignments.Cache.GetById(request.Id);
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

        public static ProductionLineAssignmentResponse Map(this ProductionLineAssignment row)
        {
            return new()
            {
                Id = row.Id,
                PlanId = row.PlanId,
                ProductionLine = row.ProductionLine == null ? new() : row.ProductionLine.Map(),
                ScheduleItems = row.ProductionScheduleItems.OrderBy(x => x.Order).Select(x => x.Map()).ToList(),
                InitialLevelWips = row.InitialLevelWips.Select(x => x.Map()).ToList(),

            };
        }
        public static InitialLevelWipResponse Map(this InitialLevelWip row)
        {
            return new()
            {
                Id = row.Id,
                InitialLevelValue = row.InitialLevel,
                InitialLevelUnit = row.InitialLevelUnit,
                WipTankLine = row.WIPTankLine == null ? new() : row.WIPTankLine.Map(),
                ProductionLineAssignmentId = row.ProductionLineAssignmentId,

            };
        }

    }

}
