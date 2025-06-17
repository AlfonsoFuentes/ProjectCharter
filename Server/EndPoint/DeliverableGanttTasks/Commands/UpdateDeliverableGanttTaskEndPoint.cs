using Server.Database.Entities.BudgetItems;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItemNewGanttTasks.Responses;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.MainTaskDependencys;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.DeliverableGanttTasks.Commands
{
    public static class UpdateDeliverableGanttTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.DeliverableGanttTasks.EndPoint.UpdateAll, async (DeliverableGanttTaskResponseList Data, IRepository Repository) =>
                {
                    await RemoveRows(Data, Repository);

                    await UpdateRows(Data, Repository);




                    var cacheDeliverable = StaticClass.DeliverableGanttTasks.Cache.Key(Data.ProjectId);
                    string cacheBudget = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);
                    List<string> cache = [.. cacheDeliverable, cacheBudget];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
            async Task RemoveRows(DeliverableGanttTaskResponseList Data, IRepository Repository)
            {
                Func<IQueryable<Deliverable>, IIncludableQueryable<Deliverable, object>> includes = x => x
               .Include(x => x.NewGanttTasks).ThenInclude(x => x.SubTasks)
                .Include(x => x.NewGanttTasks).ThenInclude(x => x.BudgetItemNewGanttTasks);
                Expression<Func<Deliverable, bool>> criteria = x => x.ProjectId == Data.ProjectId;
                var deliverables = await Repository.GetAllAsync(Criteria: criteria, Includes: includes);

                var deliverablesDTO = Data.Items.Where(x => x.IsDeliverable == true).ToList();
                var tasksDTO = Data.Items.Where(x => x.IsTask == true).ToList();
                foreach (var deliverable in deliverables)
                {
                    if (!deliverablesDTO.Any(x => x.Id == deliverable.Id))
                    {
                        await Repository.RemoveAsync(deliverable);
                    }
                    if (deliverable.NewGanttTasks.Any())
                    {
                        await RemoveSubTask(tasksDTO, deliverable.NewGanttTasks, Repository);
                    }

                }
            }
            async Task RemoveSubTask(List<DeliverableGanttTaskResponse> Data, List<NewGanttTask> subTasks, IRepository Repository)
            {
                foreach (var task in subTasks)
                {
                    if (!Data.Any(x => x.Id == task.Id))
                    {
                        if (task.BudgetItemNewGanttTasks.Any())
                        {
                            foreach (var budget in task.BudgetItemNewGanttTasks)
                            {
                                await Repository.RemoveAsync(budget);
                            }
                        }
                        await Repository.RemoveAsync(task);
                    }
                    if (task.SubTasks.Any())
                    {
                        await RemoveSubTask(Data, task.SubTasks, Repository);
                    }
                    
                }
            }
            async Task UpdateRows(DeliverableGanttTaskResponseList Data, IRepository Repository)
            {
                foreach (var data in Data.OrderedItems)
                {
                    if (data.IsDeliverable)
                    {
                        if (data.Id == Guid.Empty)
                        {
                            await CreateDeliverable(data, Data.ProjectId, Repository);
                        }
                        else
                        {
                            await UpdateDeliverable(data, Repository);

                        }
                    }
                    if (data.IsTask)
                    {
                        if (data.Id == Guid.Empty)
                        {
                            await CreateTask(data, Repository);
                        }
                        else
                        {
                            await UpdateTask(data, Repository);
                        }
                    }

                }
            }
            async Task CreateDeliverable(DeliverableGanttTaskResponse data, Guid ProjectId, IRepository Repository)
            {
                Deliverable? row = null!;
                var lastorder = await Repository.GetLastOrderAsync<Deliverable, Project>(ProjectId);
                row = Deliverable.Create(ProjectId, lastorder);
                data.MapDeliverable(row);

                await Repository.AddAsync(row);
            }
            async Task CreateTask(DeliverableGanttTaskResponse data, IRepository Repository)
            {
                NewGanttTask? row = null!;
                if (data.TaskParentId.HasValue)
                {
                    if (data.IsParentDeliverable)
                    {
                        row = NewGanttTask.Create(data.DeliverableId);
                    }
                    else
                    {
                        row = NewGanttTask.AddSubTask(data.TaskParentId.Value, data.DeliverableId);
                    }
                    await Repository.AddAsync(row);
                    data.MapTask(row);
                    await CreateTaskBudgetItems(row, data, Repository);
                    await CreateDependency(row, data, Repository);
                }


            }

            async Task UpdateDeliverable(DeliverableGanttTaskResponse data, IRepository Repository)
            {
                var deliverable = await Repository.GetByIdAsync<Deliverable>(data.Id);
                if (deliverable == null) { return; }
                data.MapDeliverable(deliverable);
                await Repository.UpdateAsync(deliverable);
            }
            async Task UpdateTask(DeliverableGanttTaskResponse data, IRepository Repository)
            {
                Func<IQueryable<NewGanttTask>, IIncludableQueryable<NewGanttTask, object>> includes = x => x
                .Include(x => x.BudgetItemNewGanttTasks).ThenInclude(x => x.SelectedBasicEngineeringItem)
                .Include(x => x.MainTasks);

                Expression<Func<NewGanttTask, bool>> criteria = x => x.Id == data.Id;

                var task = await Repository.GetAsync(Criteria: criteria, Includes: includes);
                if (task == null) { return; }
                data.MapTask(task);
                await UpdateBudgetItems(task, data, Repository);
                await UpdateDependencys(task, data, Repository);
                await Repository.UpdateAsync(task);
            }
            async Task CreateTaskBudgetItems(NewGanttTask? row, DeliverableGanttTaskResponse data, IRepository Repository)
            {
                foreach (var budget in data.OrderedBudgetItemGanttTasks)
                {
                    if (budget.BudgetItemId != Guid.Empty && row != null)
                    {
                        var item = BudgetItemNewGanttTask.Create(budget.BudgetItemId, row.Id);
                        budget.Map(item);

                        await Repository.AddAsync(item);
                        if (budget.SelectedEngineeringItemsBudgetId != Guid.Empty)
                        {
                            item.SelectedBasicEngineeringItemId = budget.SelectedEngineeringItemsBudgetId;

                        }

                    }

                }
            }
            async Task UpdateBudgetItems(NewGanttTask? row, DeliverableGanttTaskResponse data, IRepository Repository)
            {
                if (row == null) { return; }


                foreach (var budgetitemItemNewGanttTasks in row.BudgetItemNewGanttTasks)
                {
                    if (data.BudgetItemGanttTasks.Any(x => x.BudgetItemId == budgetitemItemNewGanttTasks.BudgetItemId && x.GanttTaskId == budgetitemItemNewGanttTasks.NewGanttTaskId))
                    {
                        var datafiltered = data.BudgetItemGanttTasks.Where(x => x.BudgetItemId == budgetitemItemNewGanttTasks.BudgetItemId && x.GanttTaskId == budgetitemItemNewGanttTasks.NewGanttTaskId).ToList();
                        if (!datafiltered.Any(x => x.SelectedEngineeringItemsBudgetId == budgetitemItemNewGanttTasks.SelectedBasicEngineeringItemId))
                        {
                            await Repository.RemoveAsync(budgetitemItemNewGanttTasks);
                        }
                    }
                    else
                    {
                        await Repository.RemoveAsync(budgetitemItemNewGanttTasks);
                    }







                }


                foreach (var budget in data.OrderedBudgetItemGanttTasks)
                {
                    if (budget.SelectedEngineeringItemsBudget != null)
                    {
                        if (row!.BudgetItemNewGanttTasks.Any(x => x.BudgetItemId == budget.BudgetItemId && x.NewGanttTaskId == budget.GanttTaskId && x.SelectedBasicEngineeringItemId == budget.SelectedEngineeringItemsBudget.Id))
                        {
                            var budgetItemNewGanttTasks = row.BudgetItemNewGanttTasks.FirstOrDefault(x => x.BudgetItemId == budget.BudgetItemId && x.NewGanttTaskId == budget.GanttTaskId && x.SelectedBasicEngineeringItemId == budget.SelectedEngineeringItemsBudget.Id);
                            if (budgetItemNewGanttTasks != null)
                            {
                                budget.Map(budgetItemNewGanttTasks);
                                await Repository.UpdateAsync(budgetItemNewGanttTasks);

                            }
                        }
                        else
                        {
                            if (budget.BudgetItemId != Guid.Empty && row != null)
                            {
                                var item = BudgetItemNewGanttTask.Create(budget.BudgetItemId, row.Id);
                                budget.Map(item);
                                item.SelectedBasicEngineeringItemId = budget.SelectedEngineeringItemsBudget.Id;
                                await Repository.AddAsync(item);

                            }

                        }

                    }
                    else if (row!.BudgetItemNewGanttTasks.Any(x => x.BudgetItemId == budget.BudgetItemId && x.NewGanttTaskId == budget.GanttTaskId))
                    {
                        var budgetItemNewGanttTasks = row.BudgetItemNewGanttTasks.FirstOrDefault(x => x.BudgetItemId == budget.BudgetItemId && x.NewGanttTaskId == budget.GanttTaskId);
                        if (budgetItemNewGanttTasks != null)
                        {
                            budget.Map(budgetItemNewGanttTasks);
                            await Repository.UpdateAsync(budgetItemNewGanttTasks);

                        }
                    }
                    else
                    {
                        if (budget.BudgetItemId != Guid.Empty && row != null)
                        {
                            var item = BudgetItemNewGanttTask.Create(budget.BudgetItemId, row.Id);
                            budget.Map(item);

                            await Repository.AddAsync(item);

                        }
                    }


                }
            }
            async Task CreateDependency(NewGanttTask? row, DeliverableGanttTaskResponse data, IRepository Repository)
            {
                foreach (var dependency in data.NewDependencies)
                {
                    if (dependency.DependencyTaskId != Guid.Empty && row != null)
                    {
                        var item = MainTaskDependency.Create(row.Id, dependency.DependencyTaskId);
                        dependency.Map(item);

                        await Repository.AddAsync(item);

                    }

                }
            }
            async Task UpdateDependencys(NewGanttTask? row, DeliverableGanttTaskResponse data, IRepository Repository)
            {
                if (row == null) { return; }

                foreach (var dependency in row.MainTasks)
                {
                    if (!data.NewDependencies.Any(x => x.DependencyTaskId == dependency.DependencyTaskId
                    && x.MainTaskId == dependency.MainTaskId))
                    {
                        await Repository.RemoveAsync(dependency);
                    }
                }
                foreach (var dependency in data.NewDependencies)
                {
                    if (row!.MainTasks.Any(x => x.DependencyTaskId == dependency.DependencyTaskId))
                    {
                        var depencyTask = row.MainTasks.FirstOrDefault(x =>
                        x.DependencyTaskId == dependency.DependencyTaskId);
                        if (depencyTask != null)
                        {
                            dependency.Map(depencyTask);
                            await Repository.UpdateAsync(depencyTask);
                        }
                    }
                    else
                    {
                        if (dependency.DependencyTaskId != Guid.Empty && row != null)
                        {
                            var item = MainTaskDependency.Create(row.Id, dependency.DependencyTaskId);
                            dependency.Map(item);

                            await Repository.AddAsync(item);
                        }
                    }

                }
            }
        }


        static Deliverable MapDeliverable(this DeliverableGanttTaskResponse request, Deliverable row)
        {
            row.Name = request.Name;
            row.InternalOrder = request.InternalOrder;
            row.MainOrder = request.MainOrder;
            row.Name = request.Name;
            row.EndDate = request.StartDate;
            row.StartDate = request.EndDate;
            row.DurationInUnit = request.DurationInUnit;
            row.DurationInDays = request.DurationInDays;
            row.DurationUnit = request.DurationUnit;

            return row;
        }
        static NewGanttTask MapTask(this DeliverableGanttTaskResponse response, NewGanttTask row)
        {

            row.Name = response.Name;
            row.InternalOrder = response.InternalOrder;
            row.Name = response.Name;
            row.MainOrder = response.MainOrder;
            row.StartDate = response.StartDate!.Value;
            row.EndDate = response.EndDate!.Value;

            row.DurationInDays = response.DurationInDays;

            row.DurationUnit = response.DurationUnit;
            row.DurationInUnit = response.DurationInUnit;

            row.ParentWBS = response.ParentWBS;
            row.ParentId = response.IsParentDeliverable ? null : response.TaskParentId;

            return row;
        }
        static BudgetItemNewGanttTask Map(this BudgetItemNewGanttTaskResponse response, BudgetItemNewGanttTask budgetItemNewGantt)
        {
            budgetItemNewGantt.Order = response.Order;
            //budgetItemNewGantt.PercentageBudget = response.PercentageBudget;
            budgetItemNewGantt.GanttTaskBudgetAssigned = response.BudgetAssignedUSD;


            return budgetItemNewGantt;


        }
        static MainTaskDependency Map(this MainTaskDependencyResponse response, MainTaskDependency budgetItemNewGantt)
        {
            budgetItemNewGantt.Order = response.Order;
            budgetItemNewGantt.DependencyType = response.DependencyType.Id;
            budgetItemNewGantt.LagInDays = response.LagInDays;
            budgetItemNewGantt.LagUnit = response.LagUnit;
            budgetItemNewGantt.LagInUnits = response.LagInUnits;

            return budgetItemNewGantt;


        }

    }
}
