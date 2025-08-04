using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.Backbones;
using Shared.Models.FinishingLines.InitialLevelBigWips;
using Server.EndPoint.FinishingLines.BIGWIPTanks;
namespace Server.EndPoint.FinishingLines.InitialLevelBigWips
{
    public static class CreateUpdateInitialLevelBigWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.CreateUpdate, async (InitialLevelBigWipResponse Data, IRepository Repository) =>
                {
                    InitialLevelBigWip? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = InitialLevelBigWip.Create(Data.ProductionPlanId, Data.BigWipTank.Id);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<InitialLevelBigWip>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.InitialLevelBigWips.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static InitialLevelBigWip Map(this InitialLevelBigWipResponse request, InitialLevelBigWip row)
        {
            row.InitialLevelUnit = request.InitialLevelUnit;
            row.InitialLevel = request.InitialLevelValue;
            return row;
        }

    }
    public static class DeleteInitialLevelBigWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.Delete, async (DeleteInitialLevelBigWipRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<InitialLevelBigWip>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.InitialLevelBigWips.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupInitialLevelBigWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.DeleteGroup, async (DeleteGroupInitialLevelBigWipRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<InitialLevelBigWip>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.InitialLevelBigWips.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllInitialLevelBigWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.GetAll, async (InitialLevelBigWipGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.InitialLevelBigWips.Cache.GetAll;
                    Func<IQueryable<InitialLevelBigWip>, IIncludableQueryable<InitialLevelBigWip, object>> includes = x => x.Include(y => y.BIGWIPTank!).ThenInclude(x => x.Backbone);
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<InitialLevelBigWipResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.InitialLevelBigWips.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    InitialLevelBigWipResponseList response = new InitialLevelBigWipResponseList()
                    {
                        Items = maps
                    };
                    return Result<InitialLevelBigWipResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetInitialLevelBigWipByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.GetById, async (GetInitialLevelBigWipByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<InitialLevelBigWip, bool>> Criteria = x => x.Id == request.Id;
                    Func<IQueryable<InitialLevelBigWip>, IIncludableQueryable<InitialLevelBigWip, object>> includes = x => x.Include(y => y.BIGWIPTank!).ThenInclude(x => x.Backbone);

                    string CacheKey = StaticClass.InitialLevelBigWips.Cache.GetById(request.Id);
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

        public static InitialLevelBigWipResponse Map(this InitialLevelBigWip row)
        {
            return new()
            {
                Id = row.Id,
                InitialLevelValue = row.InitialLevel,
                BigWipTank = row.BIGWIPTank == null ? null! : row.BIGWIPTank!.Map(),

            };
        }

    }
    //public static class ValidateInitialLevelBigWipsNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.InitialLevelBigWips.EndPoint.Validate, async (ValidateInitialLevelBigWipNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<InitialLevelBigWip, bool>> CriteriaId = null!;
    //                Func<InitialLevelBigWip, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.InitialLevelBigWips.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
