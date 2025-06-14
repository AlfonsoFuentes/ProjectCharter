using Shared.Models.Apps.Records;
using Shared.Models.Apps.Responses;

namespace Server.EndPoint.Apps.Queries
{
    public static class GetCurrentProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Apps.EndPoint.GetById, async (GetCurrentProjectId request, IQueryRepository Repository) =>
                {
                    GetCurrentProjectResponse response = new()
                    {
                        CurrentProjectId = Guid.Empty,
                    };

                    string CacheKey = StaticClass.Apps.Cache.GetAll;
                    var rows = await Repository.GetAllAsync<App>(Cache: CacheKey);

                    if (rows == null)
                    {
                        return Result.Fail(request.NotFound);
                    }
                    else if (rows.Count == 1)
                    {
                        response.CurrentProjectId = rows[0].CurrentProjectId;
                    }

                    return Result.Success(response);

                });
            }
        }
    }
}
