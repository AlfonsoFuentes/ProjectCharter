using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.Backbones;
using Shared.Models.FinishingLines.ProductComponents;

namespace Server.EndPoint.FinishingLines.ProductComponents
{
    public static class CreateUpdateProductComponentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductComponents.EndPoint.CreateUpdate, async (ProductComponentResponse Data, IRepository Repository) =>
                {
                    ProductComponent? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = ProductComponent.Create(Data.Backbone!.Id, Data.ProductId);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Expression<Func<ProductComponent, bool>> Criteria = x => x.ProductId == Data.ProductId && x.BackboneId == Data.Backbone!.Id;
                        row = await Repository.GetAsync(Criteria: Criteria);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.ProductComponents.Cache.Key(row.Id), .. StaticClass.Products.Cache.Key(row.ProductId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static ProductComponent Map(this ProductComponentResponse request, ProductComponent row)
        {
            row.Percentage = request.Percentage;
            row.BackboneId = request.Backbone!.Id;
            return row;
        }

    }
    public static class DeleteProductComponentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductComponents.EndPoint.Delete, async (DeleteProductComponentRequest Data, IRepository Repository) =>
                {
                    Expression<Func<ProductComponent, bool>> Criteria = x => x.ProductId == Data.ProductId && x.BackboneId == Data.BackBoneId;
                    var row = await Repository.GetAsync(Criteria: Criteria);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.ProductComponents.Cache.Key(row.Id), .. StaticClass.Products.Cache.Key(row.ProductId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductComponentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductComponents.EndPoint.DeleteGroup, async (DeleteGroupProductComponentRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<ProductComponent>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.ProductComponents.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductComponentEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductComponents.EndPoint.GetAll, async (ProductComponentGetAll request, IQueryRepository Repository) =>
                {


                    string CacheKey = StaticClass.ProductComponents.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<ProductComponent>(CacheKey);

                    if (rows == null)
                    {
                        return Result<ProductComponentResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ProductComponents.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductComponentResponseList response = new ProductComponentResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductComponentResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductComponentByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductComponents.EndPoint.GetById, async (GetProductComponentByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<ProductComponent, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ProductComponents.Cache.GetById(request.Id);
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

        public static ProductComponentResponse Map(this ProductComponent row)
        {
            return new()
            {
                Id = row.Id,
                Percentage = row.Percentage,
                ProductId = row.ProductId,
                Backbone = row.Backbone == null ? new() : row.Backbone.Map(),

            };
        }

    }
    //public static class ValidateProductComponentsNameEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.ProductComponents.EndPoint.Validate, async (ValidateProductComponentNameRequest Data, IQueryRepository Repository) =>
    //            {
    //                Expression<Func<ProductComponent, bool>> CriteriaId = null!;
    //                Func<ProductComponent, bool> CriteriaExist = x => Data.Id == null ?
    //                x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
    //                string CacheKey = StaticClass.ProductComponents.Cache.GetAll;

    //                return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
    //            });


    //        }
    //    }



    //}
}
