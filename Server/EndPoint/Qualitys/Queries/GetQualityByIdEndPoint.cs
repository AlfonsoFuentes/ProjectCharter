using Server.Database.Entities.ProjectManagements;
using Shared.Models.Qualitys.Records;
using Shared.Models.Qualitys.Responses;

namespace Server.EndPoint.Qualitys.Queries
{
    public static class GetQualityByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Qualitys.EndPoint.GetById, async (GetQualityByIdRequest request, IQueryRepository Repository) =>
                {
                   

                    Expression<Func<Quality, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Qualitys.Cache.GetById(request.Id);
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

        public static QualityResponse Map(this Quality row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order=row.Order,
                
                

            };
        }

    }
}
