using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.SKUS;
using Shared.Models.FinishingLines.LineSpeeds;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.FinishingLines.LineSpeeds
{
    public static class CreateUpdateLineSpeedEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LineSpeeds.EndPoint.CreateUpdate, async (LineSpeedResponse Data, IRepository Repository) =>
                {
                    LineSpeed? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = LineSpeed.Create(Data.LineId, Data.Sku!.Id);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Expression<Func<LineSpeed, bool>> Criteria = x => x.SkuId == Data.Sku!.Id && x.LineId == Data.LineId;
                        row = await Repository.GetAsync(Criteria: Criteria);

                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.LineSpeeds.Cache.Key(row.Id, row.LineId), .. StaticClass.ProductionLines.Cache.Key(row.LineId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static LineSpeed Map(this LineSpeedResponse request, LineSpeed row)
        {
            row.MaxSpeed = request.MaxSpeed.Value;
            row.MaxSpeedUnit = request.MaxSpeed.UnitName;
            row.PercentageAU = request.PercentageAU;
            return row;
        }

    }
    public static class DeleteLineSpeedEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LineSpeeds.EndPoint.Delete, async (DeleteLineSpeedRequest Data, IRepository Repository) =>
                {
                    Expression<Func<LineSpeed, bool>> Criteria = x => x.LineId == Data.LineId && x.SkuId == Data.SkuId;
                    var row = await Repository.GetAsync(Criteria: Criteria);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.LineSpeeds.Cache.Key(row.Id, row.LineId), .. StaticClass.ProductionLines.Cache.Key(row.LineId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupLineSpeedEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LineSpeeds.EndPoint.DeleteGroup, async (DeleteGroupLineSpeedRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        Expression<Func<LineSpeed, bool>> Criteria = x => x.LineId == rowItem.LineId && x.SkuId == rowItem.Sku!.Id;
                        var row = await Repository.GetAsync(Criteria: Criteria);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.LineSpeeds.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllLineSpeedEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LineSpeeds.EndPoint.GetAll, async (LineSpeedGetAll request, IQueryRepository Repository) =>
                {

                    Func<IQueryable<LineSpeed>, IIncludableQueryable<LineSpeed, object>> includes = x => x
                       .Include(p => p.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!)

                        ;
                    string CacheKey = StaticClass.LineSpeeds.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<LineSpeedResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.LineSpeeds.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();
                    if (request.LineId != Guid.Empty)
                    {
                        maps = maps.Where(x => x.LineId == request.LineId).ToList();
                    }

                    LineSpeedResponseList response = new LineSpeedResponseList()
                    {
                        Items = maps
                    };
                    return Result<LineSpeedResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetLineSpeedByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.LineSpeeds.EndPoint.GetById, async (GetLineSpeedByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<LineSpeed>, IIncludableQueryable<LineSpeed, object>> includes = x => x
                       .Include(p => p.Sku).ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!);

                    Expression<Func<LineSpeed, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.LineSpeeds.Cache.GetById(request.Id);
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

        public static LineSpeedResponse Map(this LineSpeed row)
        {
            return new()
            {
                Id = row.Id,
                LineId = row.LineId,
                Sku = row.Sku.Map(),
                MaxSpeedValue = row.MaxSpeed,
          
                PercentageAU = row.PercentageAU,

            };
        }

    }
    //public static class ValidateLineSpeedsNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.LineSpeeds.EndPoint.Validate, async (ValidateLineSpeedNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<LineSpeed, bool>> CriteriaId = null!;
    //                Func<LineSpeed, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.LineSpeeds.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
