using Server.Database.Entities.ProjectManagements;
using Server.EndPoint.Projects.Queries;
using Shared.Models.KnownRisks.Records;

namespace Server.EndPoint.KnownRisks.Queries
{
    public static class GetKnownRiskByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.KnownRisks.EndPoint.GetById, async (GetKnownRiskByIdRequest request, IQueryRepository Repository) =>
                {
                   

                    Expression<Func<KnownRisk, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.KnownRisks.Cache.GetById(request.Id);
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
        public static KnownRiskResponse Map(this KnownRisk row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order = row.Order,
                
                
            };
        }


    }
}
