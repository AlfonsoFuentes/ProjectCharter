using Server.Database.FinishlinLines;
using Shared.Models.FinishingLines.ProductComponents;
using Shared.Models.FinishingLines.Products;
using Server.EndPoint.FinishingLines.Backbones;

namespace Server.EndPoint.FinishingLines.Products
{
    public static class CreateUpdateProductEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.CreateUpdate, async (ProductResponse Data, IRepository Repository) =>
                {
                    Product? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Product.Create();
                        foreach (var item in Data.Components)
                        {
                            var component = ProductComponent.Create(item.Backbone!.Id, row.Id);
                            component.Percentage = item.Percentage;
                            await Repository.AddAsync(component);
                        }

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes = x => x
                        .Include(p => p.Components).ThenInclude(x => x.Backbone!)
                         ;
                        Expression<Func<Product, bool>> Criteria = x => x.Id == Data.Id;
                        row = await Repository.GetAsync(Criteria: Criteria, Includes: includes);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                        foreach (var item in Data.Components)
                        {
                            var component = row.Components.FirstOrDefault(x => x.Id == item.Id);
                            if (component == null)
                            {
                                component = ProductComponent.Create(item.Backbone!.Id, row.Id);
                                component.Percentage = item.Percentage;
                                await Repository.AddAsync(component);
                            }
                            else
                            {
                                component.BackboneId = item.Backbone!.Id;
                                component.Percentage = item.Percentage;
                                await Repository.UpdateAsync(component);
                            }
                        }
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Products.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static Product Map(this ProductResponse request, Product row)
        {
            row.Name = request.Name;

            return row;
        }

    }
    public static class DeleteProductEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.Delete, async (DeleteProductRequest Data, IRepository Repository) =>
                {
                    Expression<Func<Product, bool>> Criteria = x => x.Id == Data.Id;
                    var row = await Repository.GetAsync(Criteria: Criteria);

                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.Products.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.DeleteGroup, async (DeleteGroupProductRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<Product>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.Products.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.GetAll, async (ProductGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes = x => x
                       .Include(p => p.Components).ThenInclude(x => x.Backbone!)
                        ;

                    string CacheKey = StaticClass.Products.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<ProductResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Products.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductResponseList response = new ProductResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.GetById, async (GetProductByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes = x => x
                      .Include(p => p.Components).ThenInclude(x => x.Backbone!)
                       ;
                    Expression<Func<Product, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Products.Cache.GetById(request.Id);
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

        public static ProductResponse Map(this Product row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                Components = row.Components.Select(x => x.Map()).ToList(),


            };
        }
        public static ProductComponentResponse Map(this ProductComponent row)
        {
            return new()
            {
                Id = row.Id,
                Backbone = row.Backbone == null ? null! : row.Backbone!.Map(),
                Percentage = row.Percentage
            };
        }

    }
    public static class ValidateProductsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Products.EndPoint.Validate, async (ValidateProductNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Product, bool>> CriteriaId = null!;
                    Func<Product, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Products.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
