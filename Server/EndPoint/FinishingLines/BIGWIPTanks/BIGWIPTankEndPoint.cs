using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.Backbones;
using Shared.Models.FinishingLines.BIGWIPTanks;

namespace Server.EndPoint.FinishingLines.BIGWIPTanks
{
    public static class CreateUpdateBIGWIPTankEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.CreateUpdate, async (BIGWIPTankResponse Data, IRepository Repository) =>
                {
                    BIGWIPTank? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = BIGWIPTank.Create(Data.Backbone!.Id);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<BIGWIPTank>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.BIGWIPTanks.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static BIGWIPTank Map(this BIGWIPTankResponse request, BIGWIPTank row)
        {
            row.Name = request.Name;
            row.Capacity = request.CapacityValue;
            row.CapacityUnit = request.CapacityUnit;
            row.CleaningTimeUnit = request.CleaningTimeUnit;
            row.CleaningTime = request.CleaningTimeValue;
            row.InletMassFlow = request.InletFlowValue;
            row.InletMassFlowUnit = request.InletFlowUnit;
            row.OutletMassFlow = request.OutletFlowValue;
            row.OutletMassFlowUnit = request.OutletFlowUnit;
            row.MinimumTransferLevelKgPercentage = request.MinimumLevelPercentage;
            return row;
        }

    }
    public static class DeleteBIGWIPTankEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.Delete, async (DeleteBIGWIPTankRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<BIGWIPTank>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.BIGWIPTanks.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupBIGWIPTankEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.DeleteGroup, async (DeleteGroupBIGWIPTankRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<BIGWIPTank>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.BIGWIPTanks.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllBIGWIPTankEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.GetAll, async (BIGWIPTankGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.BIGWIPTanks.Cache.GetAll;
                    Func<IQueryable<BIGWIPTank>, IIncludableQueryable<BIGWIPTank, object>> includes = x => x.Include(y => y.Backbone);
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<BIGWIPTankResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.BIGWIPTanks.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    BIGWIPTankResponseList response = new BIGWIPTankResponseList()
                    {
                        Items = maps
                    };
                    return Result<BIGWIPTankResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetBIGWIPTankByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.GetById, async (GetBIGWIPTankByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<BIGWIPTank, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.BIGWIPTanks.Cache.GetById(request.Id);
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

        public static BIGWIPTankResponse Map(this BIGWIPTank row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                CapacityUnit = row.CapacityUnit,
                CapacityValue = row.Capacity,
                Backbone = row.Backbone == null ? new() : row.Backbone.Map(),
                CleaningTimeValue = row.CleaningTime,
                OutletFlowValue = row.OutletMassFlow,
                InletFlowValue = row.InletMassFlow,

                MinimumLevelPercentage = row.MinimumTransferLevelKgPercentage

            };
        }

    }
    public static class ValidateBIGWIPTanksNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BIGWIPTanks.EndPoint.Validate, async (ValidateBIGWIPTankNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<BIGWIPTank, bool>> CriteriaId = null!;
                    Func<BIGWIPTank, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.BIGWIPTanks.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
