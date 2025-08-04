using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Server.Database.FinishlinLines;
using Server.EndPoint.FinishingLines.LineSpeeds;
using Server.EndPoint.FinishingLines.WIPTankLines;
using Shared.Models.FinishingLines.ProductionLines;
namespace Server.EndPoint.FinishingLines.ProductionLines
{
    public static class CreateUpdateProductionLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.CreateUpdate, async (ProductionLineResponse Data, IRepository Repository) =>
                {
                    ProductionLine? row = null;
                    if (Data.Id == Guid.Empty)
                    {
                        row = ProductionLine.Create();

                        await Repository.AddAsync(row);
                        foreach (var rowwip in Data.WIPTanks)
                        {
                            var wip = WIPTankLine.Create(row.Id);
                            await Repository.AddAsync(wip);
                            rowwip.Map(wip);


                        }
                        foreach (var rowLineSpeed in Data.LineSpeeds)
                        {
                            LineSpeed? linespeed = LineSpeed.Create(row.Id, rowLineSpeed.Sku!.Id);
                            await Repository.AddAsync(linespeed);
                            rowLineSpeed.Map(linespeed);
                        }
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<ProductionLine>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }



                    Data.Map(row);
                    List<string> cache = [.. StaticClass.ProductionLines.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static ProductionLine Map(this ProductionLineResponse request, ProductionLine row)
        {
            row.Name = request.Name;
            row.FormatChangeTime = request.FormatChangeTimeValue;
            row.FormatChangeTimeUnit = request.FormatChangeTimeUnit;
            row.CleaningTime = request.FormatChangeTimeValue;
            row.CleaningTimeUnit = request.FormatChangeTimeUnit;
            return row;
        }

    }
    public static class DeleteProductionLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.Delete, async (DeleteProductionLineRequest Data, IRepository Repository) =>
                {
                    var row = await Repository.GetByIdAsync<ProductionLine>(Data.Id);
                    if (row == null) { return Result.Fail(Data.NotFound); }
                    await Repository.RemoveAsync(row);

                    List<string> cache = [.. StaticClass.ProductionLines.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class DeleteGroupProductionLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.DeleteGroup, async (DeleteGroupProductionLineRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<ProductionLine>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.ProductionLines.Cache.GetAll;

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }
    public static class GetAllProductionLineEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.GetAll, async (ProductionLineGetAll request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionLine>, IIncludableQueryable<ProductionLine, object>> includes = x => x
               .Include(y => y.WIPTanks)
               .Include(x => x.LineSpeeds).ThenInclude(x => x.Sku)
               .ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!);

                    string CacheKey = StaticClass.ProductionLines.Cache.GetAll;
                    var rows = await Repository.GetAllAsync(Cache: CacheKey, Includes: includes);

                    if (rows == null)
                    {
                        return Result<ProductionLineResponseList>.Fail(
                        StaticClass.ResponseMessages.ReponseNotFound(StaticClass.ProductionLines.ClassLegend));
                    }

                    var maps = rows.Select(x => x.Map()).ToList();


                    ProductionLineResponseList response = new ProductionLineResponseList()
                    {
                        Items = maps
                    };
                    return Result<ProductionLineResponseList>.Success(response);

                });
            }
        }
    }
    public static class GetProductionLineByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.GetById, async (GetProductionLineByIdRequest request, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ProductionLine>, IIncludableQueryable<ProductionLine, object>> includes = x => x
                    .Include(y => y.WIPTanks)
                    .Include(x => x.LineSpeeds).ThenInclude(x => x.Sku)
                    .ThenInclude(x => x.Product!).ThenInclude(x => x.Components).ThenInclude(x => x.Backbone!);

                    Expression<Func<ProductionLine, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.ProductionLines.Cache.GetById(request.Id);
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

        public static ProductionLineResponse Map(this ProductionLine row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                WIPTanks = row.WIPTanks.Select(x => x.Map()).ToList(),
                LineSpeeds = row.LineSpeeds.Select(x => x.Map()).ToList(),

                FormatChangeTimeValue = row.FormatChangeTime,
                CleaningTimeValue = row.CleaningTime,
                
            };
        }

    }
    public static class ValidateProductionLinesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ProductionLines.EndPoint.Validate, async (ValidateProductionLineNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<ProductionLine, bool>> CriteriaId = null!;
                    Func<ProductionLine, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.ProductionLines.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
