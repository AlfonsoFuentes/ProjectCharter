using Shared.Models.MonitoringExpendingTools.Responses;

namespace Server.EndPoint.MonitoringExpendingTools.Queries
{
    using Server.EndPoint.MonitoringExpendingToolAllProjects.Queries;
    using Shared.Models.MonitoringExpendingProjectTool.Records;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    namespace BudgetMonitoring
    {
        // === Dummy Entities & Interfaces (simulando tu modelo) ===

        public static class GetAllMonitoringExpendingToolAllProjectsEndPoint
        {
            private static readonly CultureInfo _CurrentCulture = new("en-US");

            public class EndPoint : IEndPoint
            {
                public void MapEndPoint(IEndpointRouteBuilder app)
                {
                    app.MapPost(StaticClass.MonitoringExpendingToolProjects.EndPoint.GetAll,
                        async (GetAllMonitoringExpendingProjectTool request, IQueryRepository repository, IRepository alternrepository) =>
                        {
                            string cacheKey = StaticClass.Projects.Cache.GetAllMonitoring;
                            Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                            .Include(x => x.PurchaseOrders).ThenInclude(x=>x.Supplier)
                            .Include(x => x.PurchaseOrders).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!)
                            .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!)
                            .Include(x => x.BudgetItems).ThenInclude(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.NewGanttTask)
                            //.Include(x => x.BudgetItems).ThenInclude(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.SelectedBasicEngineeringItem!)
                            ;

                            Expression<Func<Project, bool>> criteria = x => x.Status == ProjectStatusEnum.Approved.Id;
                            var projects = await repository.GetAllAsync(Cache: cacheKey, Criteria: criteria, Includes: includes);
                 
                            if (projects is null || !projects.Any())
                                return Result<ProjectMonitoringReportDtoList>.Fail(request.NotFound);

                            var service = new ProjectReportService(_CurrentCulture);
                            var result = new ProjectMonitoringReportDtoList
                            {
                                Items = service.Generate(projects, DateTime.Now)
                            };

                            return Result<ProjectMonitoringReportDtoList>.Success(result);
                        });
                }
            }
            
        }

    }
}