
using Server.EndPoint.BudgetItems.IndividualItems.Contingencys.Queries;

namespace Server.EndPoint.GanttTasks.Queries
{


    //public static class GetAllGanttTaskEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.GanttTasks.EndPoint.GetAll, async (GanttTaskGetAll request, IQueryRepository repository) =>
    //            {
    //                try
    //                {
    //                    Stopwatch sw = Stopwatch.StartNew();
    //                    // Validación inicial
    //                    if (!ValidateRequest(request, repository, out var validationError))
    //                    {
    //                        return Result<DeliverableWithGanttTaskResponseListToUpdate>.Fail(validationError);
    //                    }

    //                    // Cargar datos
    //                    var project = await LoadAllGanttTasksFlat(request.ProjectId, repository);
    //                    if (project == null || !project.Deliverables.Any())
    //                    {
    //                        return Result<DeliverableWithGanttTaskResponseListToUpdate>.Fail("No deliverables found.");
    //                    }

    //                    DeliverableWithGanttTaskResponseListToUpdate response = new();
    //                    response.ProjectId = request.ProjectId;
                     
    //                    foreach (var deliverable in project.Deliverables)
    //                    {
    //                        var flatList = await MapFlatInParallelAsync(deliverable.GanttTasks);

    //                        // Reconstruir jerarquía en paralelo
    //                        var hierarchy = RebuildHierarchyInParallel(flatList);

    //                        // Reconstruir dependencias en paralelo
    //                        RebuildDependantsInParallel(flatList, deliverable.GanttTasks);
    //                        DeliverableWithGanttTaskResponseToUpdate deliverableDto = new()
    //                        {
    //                            DeliverableId = deliverable.Id,
    //                            FlatOrderedItems = flatList,
    //                            Items = hierarchy,
    //                            Name = deliverable.Name,
    //                            ProjectId = deliverable.ProjectId,
    //                            LabelOrder = deliverable.Order,
    //                            WBS = $"{deliverable.Order}",
    //                            IsExpanded = deliverable.IsExpanded,
    //                            ProjectStartDate = project.StartDate,


    //                        };
    //                        response.Deliverables.Add(deliverableDto);
    //                    }
    //                    // Mapear en paralelo


    //                    sw.Stop();

    //                    Console.WriteLine($"Consulting GanttTask GetAll By Project {sw.ElapsedMilliseconds} ms");

    //                    return Result<DeliverableWithGanttTaskResponseListToUpdate>.Success(response);
    //                }
    //                catch (Exception ex)
    //                {
    //                    return Result<DeliverableWithGanttTaskResponseListToUpdate>.Fail($"An error occurred: {ex.Message}");
    //                }
    //            });
    //        }

    //        private static bool ValidateRequest(GanttTaskGetAll request, IQueryRepository repository, out string errorMessage)
    //        {
    //            if (request == null || repository == null)
    //            {
    //                errorMessage = "Invalid request: Request or repository is null.";
    //                return false;
    //            }
    //            if (request.ProjectId == Guid.Empty)
    //            {
    //                errorMessage = "Invalid request: Missing or invalid ProjectId.";
    //                return false;
    //            }
    //            errorMessage = null!;
    //            return true;
    //        }

    //        private static async Task<Project> LoadAllGanttTasksFlat(Guid projectId, IQueryRepository repository)
    //        {
    //            var cache = StaticClass.GanttTasks.Cache.GetAll(projectId);

    //            if (cache == null)
    //            {
    //                return null!;
    //            }
    //            Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
    //            .Include(x => x.Deliverables).ThenInclude(x => x.GanttTasks).ThenInclude(x => x.BudgetItems)
    //                   .Include(x => x.Deliverables).ThenInclude(x => x.GanttTasks).ThenInclude(x => x.Dependants)
    //                   .Include(x => x.Deliverables).ThenInclude(x => x.GanttTasks).ThenInclude(x => x.SubTasks);

    //            Expression<Func<Project, bool>> criteria = x => x.Id == projectId;
    //            var project = await repository.GetAsync(Cache: cache, Criteria: criteria, Includes: includes);





    //            return project!;
    //        }

    //        private static async Task<List<GanttTaskResponse>> MapFlatInParallelAsync(List<GanttTask> rows)
    //        {
    //            var tasks = rows.Select(async row =>
    //            {
    //                return await Task.Run(() => row.MapParallel());
    //            });

    //            // Convertir el resultado de Task.WhenAll (arreglo) a una lista
    //            return (await Task.WhenAll(tasks)).ToList();
    //        }

    //        private static List<GanttTaskResponse> RebuildHierarchyInParallel(List<GanttTaskResponse> flatList)
    //        {
    //            var idToItemMap = flatList.ToDictionary(item => item.Id);
    //            var rootItems = new ConcurrentBag<GanttTaskResponse>();

    //            Parallel.ForEach(flatList, item =>
    //            {
    //                if (!item.ParentGanttTaskId.HasValue)
    //                {
    //                    rootItems.Add(item);
    //                }
    //                else if (idToItemMap.TryGetValue(item.ParentGanttTaskId.Value, out var parent))
    //                {
    //                    lock (parent.SubGanttTasks)
    //                    {
    //                        parent.SubGanttTasks.Add(item);
    //                    }
    //                }
    //                else
    //                {
    //                    //throw new InvalidOperationException($"ParentGanttTaskId '{item.ParentGanttTaskId}' not found.");
    //                }
    //            });

    //            return rootItems.ToList();
    //        }

    //        private static void RebuildDependantsInParallel(List<GanttTaskResponse> flatList, List<GanttTask> rows)
    //        {
    //            var flatListDict = flatList.ToDictionary(x => x.Id);

    //            Parallel.ForEach(rows.Where(r => r.Dependants.Any()), row =>
    //            {
    //                if (flatListDict.TryGetValue(row.Id, out var mappedRow))
    //                {
    //                    foreach (var dependant in row.Dependants)
    //                    {
    //                        if (flatListDict.TryGetValue(dependant.Id, out var mappedDependant))
    //                        {
    //                            lock (mappedRow.Dependants)
    //                            {
    //                                mappedRow.Dependants.Add(mappedDependant);
    //                            }
    //                        }
    //                    }
    //                }
    //            });
    //        }
    //    }
    //}

    //// Extension method para mapear un GanttTask a GanttTaskResponse
    //public static class GanttTaskExtensions
    //{
    //    public static GanttTaskResponse MapParallel(this GanttTask row)
    //    {
    //        return new GanttTaskResponse
    //        {
    //            ProjectId = row.ProjectId,
    //            DeliverableId = row.Deliverable.Id,
    //            DependencyType = string.IsNullOrEmpty(row.DependencyType)
    //                ? TasksRelationTypeEnum.FinishStart
    //                : TasksRelationTypeEnum.GetType(row.DependencyType),
    //            StartDate = row.PlannedStartDate,
    //            EndDate = row.PlannedEndDate,
    //            Duration = row.Duration,
    //            Lag = row.Lag,
    //            ParentGanttTaskId = row.ParentId,
    //            Id = row.Id,
    //            Name = row.Name,
    //            Order = row.Order,
    //            IsExpanded = row.IsExpanded,
    //            WBS = row.WBS,
    
    //            LabelOrder = row.LabelOrder,
    //            DependantId = row.DependentantId,
    //            ShowBudgetItems = row.ShowBudgetItems,
    //            Alterations = row.BudgetItems?.OfType<Alteration>().Select(x => x.Map()).ToList() ?? new(),
    //            Structurals = row.BudgetItems?.OfType<Structural>().Select(x => x.Map()).ToList() ?? new(),
    //            Foundations = row.BudgetItems?.OfType<Foundation>().Select(x => x.Map()).ToList() ?? new(),
    //            Equipments = row.BudgetItems?.OfType<Equipment>().Select(x => x.Map()).ToList() ?? new(),

    //            Valves = row.BudgetItems?.OfType<Valve>().Select(x => x.Map()).ToList() ?? new(),
    //            Electricals = row.BudgetItems?.OfType<Electrical>().Select(x => x.Map()).ToList() ?? new(),
    //            Pipings = row.BudgetItems?.OfType<Pipe>().Select(x => x.Map()).ToList() ?? new(),
    //            Instruments = row.BudgetItems?.OfType<Instrument>().Select(x => x.Map()).ToList() ?? new(),

    //            EHSs = row.BudgetItems?.OfType<EHS>().Select(x => x.Map()).ToList() ?? new(),
    //            Paintings = row.BudgetItems?.OfType<Painting>().Select(x => x.Map()).ToList() ?? new(),
    //            Taxes = row.BudgetItems?.OfType<Tax>().Select(x => x.Map()).ToList() ?? new(),
    //            Testings = row.BudgetItems?.OfType<Testing>().Select(x => x.Map()).ToList() ?? new(),

    //            EngineeringDesigns = row.BudgetItems?.OfType<EngineeringDesign>().Select(x => x.Map()).ToList() ?? new(),

    //        };
    //    }
    //}
}
