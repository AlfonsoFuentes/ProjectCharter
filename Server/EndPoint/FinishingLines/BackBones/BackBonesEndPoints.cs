using Server.Database.FinishlinLines;
using Shared.Models.FinishingLines.BackBones;

namespace Server.EndPoint.FinishingLines.Backbones
{
    public static class CreateUpdateBackboneEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.CreateUpdate, async (BackBoneResponse Data, IRepository Repository) =>
                {
                    Backbone? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Backbone.Create();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Backbone>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Backbones.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static Backbone Map(this BackBoneResponse request, Backbone row)
        {
            row.Name = request.Name;
            return row;
        }

    }
    public static class DeleteBackboneEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.Delete, async (DeleteBackBoneRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<Backbone>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.Backbones.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupBackboneEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.DeleteGroup, async (DeleteGroupBackBoneRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<Backbone>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.Backbones.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllBackboneEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.GetAll, async (BackBoneGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.Backbones.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<Backbone>(CacheKey);

                    if (rows == null)
                    {
                        return Result<BackBoneResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Backbones.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    BackBoneResponseList response = new BackBoneResponseList()
                    {
                        Items = maps
                    };
                    return Result<BackBoneResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetBackboneByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.GetById, async (GetBackBoneByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<Backbone, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Backbones.Cache.GetById(request.Id);
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

        public static BackBoneResponse Map(this Backbone row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,

            };
        }

    }
    public static class ValidateBackbonesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Backbones.EndPoint.Validate, async (ValidateBackBoneNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Backbone, bool>> CriteriaId = null!;
                    Func<Backbone, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Backbones.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
