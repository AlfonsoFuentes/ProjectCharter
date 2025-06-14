using Server.Database.Entities.ProjectManagements;

namespace Server.EndPoint.Bennefits.Queries
{
    public static class GetAllBennefitEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Bennefits.EndPoint.GetAll, async (BennefitGetAll request, IQueryRepository repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x.Include(p => p.Bennefits);
                    Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                    string cacheKey = StaticClass.Bennefits.Cache.GetAll(request.ProjectId);

                    var rows = await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);

                    if (rows == null)
                    {
                        return Result<BennefitResponseList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.Bennefits.ClassLegend));
                    }

                    var maps = rows.Bennefits.OrderBy(x => x.Order).Select(x => x.Map()).ToList();

                    var response = new BennefitResponseList
                    {
                        Items = maps,
                      ProjectId = request.ProjectId,
                    };

                    return Result<BennefitResponseList>.Success(response);
                });
            }

            private static async Task<List<Bennefit>> GetBennefitAsync(BennefitGetAll request, IQueryRepository repository)
            {

                Expression<Func<Bennefit, bool>> criteria = x => x.ProjectId == request.ProjectId;
                string cacheKey = StaticClass.Bennefits.Cache.GetAll(request.ProjectId);

                return await repository.GetAllAsync(Cache: cacheKey, Criteria: criteria);
            }


        }
    }
}