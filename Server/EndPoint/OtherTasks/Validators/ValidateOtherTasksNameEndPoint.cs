using Server.Database.Entities.ProjectManagements;
using Shared.Models.OtherTasks.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.OtherTasks.Validators
{
    public static class ValidateOtherTasksNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.OtherTasks.EndPoint.Validate, async (ValidateOtherTaskRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<OtherTask, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<OtherTask, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.OtherTasks.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
