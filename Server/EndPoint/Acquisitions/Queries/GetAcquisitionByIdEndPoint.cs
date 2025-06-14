using Server.Database.Entities.ProjectManagements;
using Shared.Models.Acquisitions.Records;
using Shared.Models.Acquisitions.Responses;

namespace Server.EndPoint.Acquisitions.Queries
{
    public static class GetAcquisitionByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Acquisitions.EndPoint.GetById, async (GetAcquisitionByIdRequest request, IQueryRepository Repository) =>
                {
                    //Func<IQueryable<Acquisition>, IIncludableQueryable<Acquisition, object>> Includes = x => null!

                    //;

                    Expression<Func<Acquisition, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Acquisitions.Cache.GetById(request.Id);
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

        public static AcquisitionResponse Map(this Acquisition row)
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
