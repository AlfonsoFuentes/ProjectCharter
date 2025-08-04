using DocumentFormat.OpenXml.Spreadsheet;
using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.MixerBackbones;
using Shared.Models.FinishingLines.MixerBackbones;
using Shared.Models.FinishingLines.Mixers;
namespace Server.EndPoint.FinishingLines.Mixers
{
    public static class CreateUpdateMixerEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.CreateUpdate, async (MixerResponse Data, IRepository Repository) =>
                {
                    Mixer? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Mixer.Create();

                        await Repository.AddAsync(row);
                        foreach (var mixerbackbone in Data.Capabilities)
                        {
                            var newMixerBackbone = MixerBackbone.Create(row.Id, mixerbackbone.Backbone!.Id);
                            mixerbackbone.Map(newMixerBackbone);

                            await Repository.AddAsync(newMixerBackbone);
                        }
                    }
                    else
                    {
                        Expression<Func<Mixer, bool>> Criteria = x => x.Id == Data.Id;
                        //Func<IQueryable<Mixer>, IIncludableQueryable<Mixer, object>> includes = x => x
                        //.Include(p => p.Capabilities).ThenInclude(x => x.Backbone)
                         ;
                        row = await Repository.GetAsync(Criteria: Criteria);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }






                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Mixers.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static Mixer Map(this MixerResponse request, Mixer row)
        {
            row.Name = request.Name;

            row.CleaningTime = request.CleaningTimeValue;
            row.CleaningTimeUnit = request.CleaningTimeUnit;
            return row;
        }

    }
    public static class DeleteMixerEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.Delete, async (DeleteMixerRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Mixer>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.Mixers.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupMixerEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.DeleteGroup, async (DeleteGroupMixerRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<Mixer>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.Mixers.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllMixerEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.GetAll, async (MixerGetAll request, IQueryRepository Repository) =>
                {

                    Func<IQueryable<Mixer>, IIncludableQueryable<Mixer, object>> includes = x => x
                   .Include(p => p.Capabilities).ThenInclude(x => x.Backbone)
                    ;
                    string CacheKey = StaticClass.Mixers.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<MixerResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Mixers.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    MixerResponseList response = new MixerResponseList()
                    {
                        Items = maps
                    };
                    return Result<MixerResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetMixerByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.GetById, async (GetMixerByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<Mixer, bool>> Criteria = x => x.Id == request.Id;
                    Func<IQueryable<Mixer>, IIncludableQueryable<Mixer, object>> includes = x => x
                    .Include(p => p.Capabilities).ThenInclude(x => x.Backbone)
                     ;
                    string CacheKey = StaticClass.Mixers.Cache.GetById(request.Id);
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

        public static MixerResponse Map(this Mixer row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,

                CleaningTimeValue = row.CleaningTime,

                Capabilities = row.Capabilities == null ? new() : row.Capabilities.Select(x => x.Map()).ToList()


            };
        }

    }
    public static class ValidateMixersNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Mixers.EndPoint.Validate, async (ValidateMixerNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Mixer, bool>> CriteriaId = null!;
                    Func<Mixer, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Mixers.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
