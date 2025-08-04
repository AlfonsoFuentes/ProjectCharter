using Server.Database.FinishlinLines;
using Shared.Models.FinishingLines.WIPTankLines;

namespace Server.EndPoint.FinishingLines.WIPTankLines
{
    public static class CreateUpdateWIPTankLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.WIPTankLines.EndPoint.CreateUpdate, async (WIPTankLineResponse Data, IRepository Repository) =>
                {
                    WIPTankLine? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = WIPTankLine.Create(Data.LineId);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<WIPTankLine>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.WIPTankLines.Cache.Key(row.Id, row.LineId), .. StaticClass.ProductionLines.Cache.Key(row.LineId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static WIPTankLine Map(this WIPTankLineResponse request, WIPTankLine row)
        {
            row.Name = request.Name;
            row.Capacity = request.Capacity.Value;
            row.CapacityUnit = request.Capacity.UnitName;
            row.CleaningTimeUnit = request.CleaningTimeUnit;
            row.CleaningTime = request.CleaningTimeValue;
            row.MinimumLevelPercentage = request.MinimumLevelPercentage;
            return row;
        }

    }
    public static class DeleteWIPTankLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.WIPTankLines.EndPoint.Delete, async (DeleteWIPTankLineRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<WIPTankLine>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.WIPTankLines.Cache.Key(row.Id, row.LineId), .. StaticClass.ProductionLines.Cache.Key(row.LineId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupWIPTankLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.WIPTankLines.EndPoint.DeleteGroup, async (DeleteGroupWIPTankLineRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<WIPTankLine>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.WIPTankLines.Cache.GetAll(Data.LineId);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllWIPTankLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.WIPTankLines.EndPoint.GetAll, async (WIPTankLineGetAll request, IQueryRepository Repository) =>
                {
                    Expression<Func<WIPTankLine, bool>> Criteria = x => x.Id == request.ProductionLineId;

                    string CacheKey = StaticClass.WIPTankLines.Cache.GetAll(request.ProductionLineId);
                    var rows = await Repository.GetAllAsync(Cache:CacheKey,Criteria:Criteria);

                    if (rows == null)
                    {
                        return Result<WIPTankLineResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.WIPTankLines.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    WIPTankLineResponseList response = new WIPTankLineResponseList()
                    {
                        Items = maps
                    };
                    return Result<WIPTankLineResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetWIPTankLineByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.WIPTankLines.EndPoint.GetById, async (GetWIPTankLineByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<WIPTankLine, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.WIPTankLines.Cache.GetById(request.Id);
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

        public static WIPTankLineResponse Map(this WIPTankLine row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                LineId = row.LineId,
                CapacityValue = row.Capacity,

                CleaningTimeValue = row.CleaningTime,
                MinimumLevelPercentage = row.MinimumLevelPercentage,


            };
        }

    }
    //public static class ValidateWIPTankLinesNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.WIPTankLines.EndPoint.Validate, async (ValidateWIPTankLineNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<WIPTankLine, bool>> CriteriaId = null!;
    //                Func<WIPTankLine, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.WIPTankLines.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
