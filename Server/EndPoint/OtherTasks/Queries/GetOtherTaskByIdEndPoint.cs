using Server.Database.Entities.ProjectManagements;
using Shared.Enums.OtherTask;
using Shared.Models.OtherTasks.Records;
using Shared.Models.OtherTasks.Responses;

namespace Server.EndPoint.OtherTasks.Queries
{
    public static class GetOtherTaskByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.GetById, async (GetOtherTaskByIdRequest request, IQueryRepository Repository) =>
                {
                    //Func<IQueryable<OtherTask>, IIncludableQueryable<OtherTask, object>> Includes = x => null!

                    //;

                    Expression<Func<OtherTask, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.OtherTasks.Cache.GetById(request.Id);
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

        public static OtherTaskResponse Map(this OtherTask row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ProjectId = row.ProjectId,
                Order = row.Order,
                StartDate = row.StartDate,
                EndDate = row.EndDate,
                //Status = OtherTaskStatusEnum.GetType(row.Status),
                CECName = row.CECName ?? string.Empty,
                ProjectName = row.ProjectName ?? string.Empty,


            };
        }

    }
}
