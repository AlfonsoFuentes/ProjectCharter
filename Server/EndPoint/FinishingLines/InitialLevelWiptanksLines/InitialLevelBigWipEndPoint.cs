using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.Backbones;
using Shared.Models.FinishingLines.InitialLevelWips;

namespace Server.EndPoint.FinishingLines.InitialLevelWips
{
    public static class CreateUpdateInitialLevelWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelWips.EndPoint.CreateUpdate, async (InitialLevelWipResponse Data, IRepository Repository) =>
                {
                    InitialLevelWip? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = InitialLevelWip.Create(Data.ProductionLineAssignmentId, Data.WipTankLine.Id);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<InitialLevelWip>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.InitialLevelWips.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static InitialLevelWip Map(this InitialLevelWipResponse request, InitialLevelWip row)
        {
            row.InitialLevelUnit=request.InitialLevelUnit;
            row.InitialLevel = request.InitialLevelValue;
            return row;
        }

    }
    public static class DeleteInitialLevelWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelWips.EndPoint.Delete, async (DeleteInitialLevelWipRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<InitialLevelWip>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.InitialLevelWips.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupInitialLevelWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelWips.EndPoint.DeleteGroup, async (DeleteGroupInitialLevelWipRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<InitialLevelWip>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.InitialLevelWips.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllInitialLevelWipEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelWips.EndPoint.GetAll, async (InitialLevelWipGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.InitialLevelWips.Cache.GetAll;
                    Func<IQueryable<InitialLevelWip>, IIncludableQueryable<InitialLevelWip, object>> includes = x => x.Include(y => y.WIPTankLine!);
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<InitialLevelWipResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.InitialLevelWips.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    InitialLevelWipResponseList response = new InitialLevelWipResponseList()
                    {
                        Items = maps
                    };
                    return Result<InitialLevelWipResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetInitialLevelWipByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InitialLevelWips.EndPoint.GetById, async (GetInitialLevelWipByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<InitialLevelWip, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.InitialLevelWips.Cache.GetById(request.Id);
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

        public static InitialLevelWipResponse Map(this InitialLevelWip row)
        {
            return new()
            {
                Id = row.Id,
                InitialLevelValue = row.InitialLevel

            };
        }

    }
    //public static class ValidateInitialLevelWipsNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.InitialLevelWips.EndPoint.Validate, async (ValidateInitialLevelWipNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<InitialLevelWip, bool>> CriteriaId = null!;
    //                Func<InitialLevelWip, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.InitialLevelWips.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
