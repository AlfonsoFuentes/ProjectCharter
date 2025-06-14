using Server.Database.Entities.ProjectManagements;
using Shared.Models.Communications.Records;
using Shared.Models.Communications.Responses;

namespace Server.EndPoint.Communications.Queries
{
    public static class GetCommunicationByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Communications.EndPoint.GetById, async (GetCommunicationByIdRequest request, IQueryRepository Repository) =>
                {
                   

                    Expression<Func<Communication, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Communications.Cache.GetById(request.Id);
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

        public static CommunicationResponse Map(this Communication row)
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
