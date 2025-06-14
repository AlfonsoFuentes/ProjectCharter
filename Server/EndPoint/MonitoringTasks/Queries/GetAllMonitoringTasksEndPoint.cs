using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.MonitoringTask.Records;
using Shared.Models.MonitoringTask.Responses;
namespace Server.EndPoint.MonitoringTasks.Queries
{

    public static class GetAllMonitoringTasksEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.MonitoringTasks.EndPoint.GetAll, async (GetAllMonitoringGanttTask request, IQueryRepository repository) =>
                {
                    try
                    {

                        if (!ValidateRequest(request, repository, out var validationError))
                        {
                            return Result<MonitoringGanttTaskResponseList>.Fail(validationError);
                        }

                        //Cargar datos
                        var project = await LoadAllGanttTasksFlat(request.ProjectId, repository);
                        if (project == null)
                        {
                            return Result<MonitoringGanttTaskResponseList>.Fail(request.NotFound);
                        }

                        MonitoringGanttTaskResponseList response = new();
                        response.ProjectId = request.ProjectId;
                        response.ProjectName = project.Name;

                        foreach (var deliverable in project.Deliverables)
                        {
                            MonitoringGanttTaskResponse deliverableDto = new()
                            {

                                IsDeliverable = true,
                                InternalOrder = deliverable.InternalOrder,
                                MainOrder = deliverable.MainOrder,
                                StartDate = deliverable.StartDate,
                                EndDate = deliverable.EndDate,
                                Id = deliverable.Id,
                                Name = deliverable.Name,
                                DeliverableId = deliverable.Id,
                                DurationInDays = deliverable.DurationInDays,
                                DurationUnit = deliverable.DurationUnit,
                                DurationInUnit = deliverable.DurationInUnit,



                            };
                            response.Items.Add(deliverableDto);
                            var flatList = MapFlatInParallelAsync(deliverable.NewGanttTasks);


                            response.Items.AddRange(flatList);



                        }



                        return Result<MonitoringGanttTaskResponseList>.Success(response);

                    }
                    catch (Exception ex)
                    {
                        return Result<MonitoringGanttTaskResponseList>.Fail($"An error occurred: {ex.Message}");
                    }
                });

            }

            private static List<MonitoringGanttTaskResponse> MapFlatInParallelAsync(List<NewGanttTask> rows)
            {

                var tasks = rows.Select(row => row.MapParallel()).ToList();


                return tasks.OrderBy(x => x.MainOrder).ToList();
            }




            private static bool ValidateRequest(GetAllMonitoringGanttTask request, IQueryRepository repository, out string errorMessage)
            {
                if (request == null || repository == null)
                {
                    errorMessage = "Invalid request: Request or repository is null.";
                    return false;
                }
                if (request.ProjectId == Guid.Empty)
                {
                    errorMessage = "Invalid request: Missing or invalid ProjectId.";
                    return false;
                }
                errorMessage = null!;
                return true;
            }

            private static async Task<Project?> LoadAllGanttTasksFlat(Guid projectId, IQueryRepository repository)
            {
                var cache = StaticClass.DeliverableGanttTasks.Cache.GetAll(projectId);

                if (cache == null)
                {
                    return null!;
                }
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                   .Include(x => x.Deliverables)
                .ThenInclude(x => x.NewGanttTasks)
                .ThenInclude(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.BudgetItem)
               
                .Include(x => x.Deliverables)
                .ThenInclude(x => x.NewGanttTasks)
                .ThenInclude(x => x.MainTasks)
                ;


                ;

                Expression<Func<Project, bool>> criteria = x => x.Id == projectId;
                var project = await repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);





                return project!;
            }
        }
        public static MonitoringGanttTaskResponse MapParallel(this NewGanttTask row)
        {
            return new MonitoringGanttTaskResponse
            {
                MainOrder = row.MainOrder,
                WBS = row.WBS,
                HasSubTask = row.SubTasks.Any(),
                Name = row.Name,
                StartDate = row.StartDate,
                EndDate = row.EndDate,
                DurationInDays = row.DurationInDays,
                DurationInUnit = row.DurationInUnit,
                DurationUnit = row.DurationUnit,
                IsDeliverable = false,
                Id = row.Id,

                DeliverableId = row.DeliverableId,
                TaskParentId = row.ParentId == null ? row.DeliverableId : row.ParentId,
                InternalOrder = row.InternalOrder,

                ParentWBS = row.ParentWBS,

                IsParentDeliverable = row.ParentId == null,
                ProjectId = row.ProjectId,
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks.Select(x => x.MapMonitoring()).ToList(),
                NewDependencies = row.MainTasks.Select(x => x.MapMonitoring()).ToList(),

            };
        }
        public static BudgetItemMonitoringNewGanttTaskResponse MapMonitoring(this BudgetItemNewGanttTask row)
        {
            return new BudgetItemMonitoringNewGanttTaskResponse()
            {
                BudgetItemId = row.BudgetItemId,
                GanttTaskId = row.NewGanttTaskId,

                Order = row.Order,
                BudgetAssignedUSD = row.GanttTaskBudgetAssigned,
                SelectedEngineeringItemsBudget= row.SelectedBasicEngineeringItem == null ? null! : row.SelectedBasicEngineeringItem.Map()

            };

        }
        public static MainTaskDependencyMonitoringResponse MapMonitoring(this MainTaskDependency row)
        {
            return new MainTaskDependencyMonitoringResponse()
            {
                MainTaskId = row.MainTaskId,
                DependencyTaskId = row.DependencyTaskId,
                DependencyType = TasksRelationTypeEnum.GetType(row.DependencyType),
                LagUnit = row.LagUnit,
                LagInDays = row.LagInDays,
                LagInUnits = row.LagInUnits,


                Order = row.Order,



            };

        }


    }
}
