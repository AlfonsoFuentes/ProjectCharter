

using DocumentFormat.OpenXml.Spreadsheet;
using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.Backbones;
using Shared.Models.FinishingLines.MixerBackbones;

namespace Server.EndPoint.FinishingLines.MixerBackbones
{
    public static class CreateUpdateMixerBackbonesEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MixerBackbones.EndPoint.CreateUpdate, async (MixerBackboneResponse Data, IRepository Repository) =>
                {
                    MixerBackbone? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = MixerBackbone.Create(Data.MixerId, Data.Backbone!.Id);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Expression<Func<MixerBackbone, bool>> Criteria = x => x.MixerId == Data.MixerId && x.BackboneId == Data.Backbone!.Id;
                        row = await Repository.GetAsync(Criteria: Criteria);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.MixerBackbones.Cache.Key(row.Id), .. StaticClass.Mixers.Cache.Key(Data.MixerId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static MixerBackbone Map(this MixerBackboneResponse request, MixerBackbone row)
        {
            row.BatchTime = request.BatchCycleTimeValue;
            row.BatchTimeUnit = request.BatchCycleTimeUnit;
            row.Capacity = request.CapacityValue;
            row.CapacityUnit = request.CapacityUnit;
            return row;
        }

    }
    public static class DeleteMixerBackbonesEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MixerBackbones.EndPoint.Delete, async (DeleteMixerBackboneRequest Data, IRepository Repository) =>
                {
                    Expression<Func<MixerBackbone, bool>> Criteria = x => x.MixerId == Data.MixerId && x.BackboneId == Data.BackboneId;
                    var row = await Repository.GetAsync(Criteria: Criteria);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.MixerBackbones.Cache.Key(row.Id), .. StaticClass.Mixers.Cache.Key(Data.MixerId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupMixerBackbonesEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MixerBackbones.EndPoint.DeleteGroup, async (DeleteGroupMixerBackboneRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        Expression<Func<MixerBackbone, bool>> Criteria = x => x.MixerId == rowItem.MixerId && x.BackboneId == rowItem.Backbone!.Id;
                        var row = await Repository.GetAsync(Criteria: Criteria);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    List<string> cache = [.. StaticClass.Mixers.Cache.Key(Data.MixerId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllMixerBackbonesEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MixerBackbones.EndPoint.GetAll, async (MixerBackboneGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.MixerBackbones.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<MixerBackbone>(CacheKey);

                    if (rows == null)
                    {
                        return Result<MixerBackboneResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.MixerBackbones.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    MixerBackboneResponseList response = new MixerBackboneResponseList()
                    {
                        Items = maps
                    };
                    return Result<MixerBackboneResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetMixerBackbonesByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MixerBackbones.EndPoint.GetById, async (GetMixerBackboneByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<MixerBackbone, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.MixerBackbones.Cache.GetById(request.Id);
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

        public static MixerBackboneResponse Map(this MixerBackbone row)
        {
            return new()
            {
                Id = row.Id,
                Backbone = row.Backbone == null ? new() : row.Backbone.Map(),
                MixerId = row.MixerId,
                BatchCycleTimeValue = row.BatchTime,
         
                CapacityValue = row.Capacity,
           

            };
        }

    }
   


    //}
}
