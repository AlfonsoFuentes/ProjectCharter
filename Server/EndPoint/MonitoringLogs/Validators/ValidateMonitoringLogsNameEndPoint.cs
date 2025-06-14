using Server.Database.Entities.ProjectManagements;
using Shared.Models.MonitoringLogs.Validators;
using Shared.Models.Backgrounds.Validators;

namespace Server.EndPoint.MonitoringLogs.Validators
{
    public static class ValidateMonitoringLogsNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringLogs.EndPoint.Validate, async (ValidateMonitoringLogRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<MonitoringLog, bool>> CriteriaId = x => x.ProjectId == Data.ProjectId;
                    Func<MonitoringLog, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.MonitoringLogs.Cache.GetAll(Data.ProjectId);

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
