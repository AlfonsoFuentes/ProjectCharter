using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.SKUS;
using Shared.Models.FinishingLines.ProductionScheduleItems;

namespace Server.EndPoint.FinishingLines.ProductionScheduleItems
{
    public static class CreateUpdateProductionScheduleItemEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.CreateUpdate, async (ProductionScheduleItemResponse Data, IRepository Repository) =>
                {
                    ProductionScheduleItem? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<ProductionScheduleItem, ProductionLineAssignment>(Data.ProductionLineAssigmentId);
                        row = ProductionScheduleItem.Create(Data.ProductionLineAssigmentId, Data.Sku!.Id, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<ProductionScheduleItem>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.ProductionScheduleItems.Cache.Key(row.Id), 
                        StaticClass.ProductionPlans.Cache.GetAll,
                        ..StaticClass.ProductionLineAssignments.Cache.Key(row.ProductionLineAssignmentId),
                        ];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static ProductionScheduleItem Map(this ProductionScheduleItemResponse request, ProductionScheduleItem row)
        {
            row.PlannedMass = request.MassPlannedValue;
            row.PlannedMassUnit = request.MassPlannedUnit;
            row.SkuId = request.Sku!.Id;
            return row;
        }

    }
    public static class DeleteProductionScheduleItemEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.Delete, async (DeleteProductionScheduleItemRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<ProductionScheduleItem>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.ProductionScheduleItems.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductionScheduleItemEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.DeleteGroup, async (DeleteGroupProductionScheduleItemRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<ProductionScheduleItem>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.ProductionScheduleItems.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductionScheduleItemEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.GetAll, async (ProductionScheduleItemGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.ProductionScheduleItems.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<ProductionScheduleItem>(CacheKey);

                    if (rows == null)
                    {
                        return Result<ProductionScheduleItemResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ProductionScheduleItems.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductionScheduleItemResponseList response = new ProductionScheduleItemResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductionScheduleItemResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductionScheduleItemByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.GetById, async (GetProductionScheduleItemByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<ProductionScheduleItem, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ProductionScheduleItems.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static ProductionScheduleItemResponse Map(this ProductionScheduleItem row)
        {
            return new()
            {
                Id = row.Id,
                MassPlannedValue = row.PlannedMass,
                MassPlannedUnit = row.PlannedMassUnit,
                ProductionLineAssigmentId = row.ProductionLineAssignmentId,
                Sku = row.SKU == null ? new() : row.SKU.Map(),
                Order = row.Order,
                ProductionLineId = row.ProductionLineAssignment?.LineId ?? Guid.Empty,


            };
        }

    }
    //public static class ValidateProductionScheduleItemsNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.ProductionScheduleItems.EndPoint.Validate, async (ValidateProductionScheduleItemNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<ProductionScheduleItem, bool>> CriteriaId = null!;
    //                Func<ProductionScheduleItem, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.ProductionScheduleItems.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
