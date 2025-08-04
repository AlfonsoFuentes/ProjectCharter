using Server.Database.FinishlinLines;
using Shared.Models.FinishingLines.SKUs;
using Server.EndPoint.FinishingLines.Products;
namespace Server.EndPoint.FinishingLines.SKUS
{
    public static class CreateUpdateSKUEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.CreateUpdate, async (SKUResponse Data, IRepository Repository) =>
                {
                    SKU? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = SKU.Create();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<SKU>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.SKUs.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static SKU Map(this SKUResponse request, SKU row)
        {
            row.Name = request.Name;
            row.MassPerEA = request.MassPerEAValue;
            row.MassPerEAUnit = request.MassPerEAUnit;
            row.VolumePerEA = request.VolumePerEAValue;
            row.VolumePerEAUnit = request.VolumePerEAUnit;
            row.ProductId = request.Product?.Id ?? Guid.Empty;
    

            return row;
        }

    }
    public static class DeleteSKUEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.Delete, async (DeleteSKURequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<SKU>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.SKUs.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupSKUEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.DeleteGroup, async (DeleteGroupSKURequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<SKU>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.SKUs.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllSKUEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.GetAll, async (SKUGetAll request, IQueryRepository Repository) =>
                {

                    List<SKU>? rows = null;
                    Func<IQueryable<SKU>, IIncludableQueryable<SKU, object>> includes = x => x
                    .Include(y => y.Product!)
                    .ThenInclude(x => x.Components)
                    .ThenInclude(x => x.Backbone!);
                    string CacheKey = StaticClass.SKUs.Cache.GetAll;
                    rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);


                    if (rows == null)
                    {
                        return Result<SKUResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.SKUs.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    SKUResponseList response = new SKUResponseList()
                    {
                        Items = maps
                    };
                    return Result<SKUResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetSKUByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.GetById, async (GetSKUByIdRequest request, IQueryRepository Repository) =>
                {

                    Func<IQueryable<SKU>, IIncludableQueryable<SKU, object>> includes = x => x


                    .Include(y => y.Product!)
                    .ThenInclude(x => x.Components)
                    .ThenInclude(x => x.Backbone!);
                    Expression<Func<SKU, bool>> Criteria = x => x.Id == request.Id;
                    string CacheKey = StaticClass.SKUs.Cache.GetById(request.Id);
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

        public static SKUResponse Map(this SKU row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,

                Product = row.Product == null ? null! : row.Product.Map(),
                MassPerEAValue = row.MassPerEA,

                VolumePerEAValue = row.VolumePerEA,

         



            };
        }

    }
    public static class ValidateSKUsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.SKUs.EndPoint.Validate, async (ValidateSKUNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<SKU, bool>> CriteriaId = null!;
                    Func<SKU, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.SKUs.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
