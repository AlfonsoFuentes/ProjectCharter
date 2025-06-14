using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.MainTaskDependencys;

namespace Server.EndPoint.DeliverableGanttTasks.Queries
{

    public static class GetAllDeliverableGanttTasksEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.DeliverableGanttTasks.EndPoint.GetAll,
                    async (GetAllDeliverableGanttTask request, IQueryRepository repository) =>
                {
                    try
                    {

                        if (!ValidateRequest(request, repository, out var validationError))
                        {
                            return Result<DeliverableGanttTaskResponseList>.Fail(validationError);
                        }

                        //Cargar datos
                        var project = await LoadAllGanttTasksFlat(request.ProjectId, repository);
                        if (project == null)
                        {
                            return Result<DeliverableGanttTaskResponseList>.Fail(request.NotFound);
                        }

                        DeliverableGanttTaskResponseList response = new();
                        response.ProjectId = request.ProjectId;
                        response.InitialProjectStartDate = project.StartDate;
                        response.ProjectName = project.Name;
                        foreach (var deliverable in project.Deliverables)
                        {
                            DeliverableGanttTaskResponse deliverableDto = new()
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



                        return Result<DeliverableGanttTaskResponseList>.Success(response);

                    }
                    catch (Exception ex)
                    {
                        return Result<DeliverableGanttTaskResponseList>.Fail($"An error occurred: {ex.Message}");
                    }
                });

            }

            private static List<DeliverableGanttTaskResponse> MapFlatInParallelAsync(List<NewGanttTask> rows)
            {

                var tasks = rows.Select(row => row.MapParallel()).ToList();


                return tasks.OrderBy(x => x.MainOrder).ToList();
            }




            private static bool ValidateRequest(GetAllDeliverableGanttTask request, IQueryRepository repository, out string errorMessage)
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
                .ThenInclude(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.SelectedBasicEngineeringItem)
                .Include(x => x.Deliverables)
                .ThenInclude(x => x.NewGanttTasks)
                .ThenInclude(x => x.MainTasks)
                ;

                Expression<Func<Project, bool>> criteria = x => x.Id == projectId;
                var project = await repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);





                return project!;
            }
        }
        public static DeliverableGanttTaskResponse MapParallel(this NewGanttTask row)
        {
            return new DeliverableGanttTaskResponse
            {
                IsDeliverable = false,
                StartDate = row.StartDate,
                EndDate = row.EndDate,
                DurationInDays = row.DurationInDays,
                DurationUnit = row.DurationUnit,

                DurationInUnit = row.DurationInUnit,

                Id = row.Id,
                Name = row.Name,
                DeliverableId = row.DeliverableId,
                TaskParentId = row.ParentId == null ? row.DeliverableId : row.ParentId,
                InternalOrder = row.InternalOrder,
                MainOrder = row.MainOrder,
                ParentWBS = row.ParentWBS,

                IsParentDeliverable = row.ParentId == null,
                ProjectId = row.ProjectId,
                BudgetItemGanttTasks = row.BudgetItemNewGanttTasks.Select(x => x.Map()).ToList(),
                NewDependencies = row.MainTasks.Select(x => x.Map()).ToList(),


            };
        }
        public static BudgetItemNewGanttTaskResponse Map(this BudgetItemNewGanttTask row)
        {
            return new BudgetItemNewGanttTaskResponse()
            {
                BudgetItemId = row.BudgetItemId,
                GanttTaskId = row.NewGanttTaskId,

                Order = row.Order,
                BudgetAssignedUSD = row.GanttTaskBudgetAssigned,
                SelectedEngineeringItemsBudgetId = row.SelectedBasicEngineeringItem == null! ? Guid.Empty : row.SelectedBasicEngineeringItem.Id,
                SelectedEngineeringItemsBudget = row.SelectedBasicEngineeringItem == null! ? null! : row.SelectedBasicEngineeringItem.Map(),

            };

        }
        public static MainTaskDependencyResponse Map(this MainTaskDependency row)
        {
            return new MainTaskDependencyResponse()
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
        public static BasicResponse Map(this BasicEngineeringItem row)
        {
            return new BasicResponse()
            {
                Id = row.Id,
                Name = row.Name,
                BudgetUSD = row.BudgetUSD,
                Order = row.Order,
                TagNumber = row.TagNumber,

            };
        }

    }
}
