namespace Server.EndPoint.GanttTasks.Commands
{
    //public static class UpdateGanttTaskEDTEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.GanttTasks.EndPoint.UpdateEDT, async (DeliverableWithGanttTaskResponseListToUpdate data, IRepository repository) =>
    //            {

    //                // Validar que se haya proporcionado un ProjectId
    //                if (data == null || data.ProjectId == Guid.Empty)
    //                {
    //                    return Result.Fail("Invalid request: Missing or invalid ProjectId.");
    //                }

    //                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
    //                    .Include(x => x.Deliverables).ThenInclude(x => x.GanttTasks).ThenInclude(x => x.Dependants)
    //                    .Include(x => x.Deliverables).ThenInclude(x => x.GanttTasks).ThenInclude(x => x.SubTasks);

    //                Expression<Func<Project, bool>> criteria = x => x.Id == data.ProjectId;
    //                var project = await repository.GetAsync(Criteria: criteria, Includes: includes);
    //                if(project==null) return Result.Fail("Invalid request: Missing or invalid ProjectId.");

    //                foreach (var deliverable in project.Deliverables)
    //                {
    //                    if (deliverable != null)
    //                    {
    //                        var deliverableDTO = data.Deliverables.FirstOrDefault(x => x.DeliverableId == deliverable.Id);
    //                        if(deliverableDTO != null)
    //                        {
    //                            deliverable.IsExpanded = deliverableDTO.IsExpanded;
    //                            await repository.UpdateAsync(deliverable);   
    //                            await UpdateMapped(repository, deliverable, deliverableDTO.FlatOrderedItems);

    //                            await UpdateDependencies(repository, deliverable, deliverableDTO.FlatOrderedItems);
    //                        }
                           

    //                    }

    //                }



    //                var cache = StaticClass.GanttTasks.Cache.GetAll(data.ProjectId);
    //                // Guardar los cambios en la base de datos y limpiar la caché
    //                var result = await repository.Context.SaveChangesAndRemoveCacheAsync(cache);


    //                return Result.EndPointResult(
    //                    result,
    //                    data.Succesfully,
    //                    data.Fail);
    //            });
    //        }
    //        async Task UpdateDependencies(IRepository repository, Deliverable deliverable, List<GanttTaskResponse> flatlist)
    //        {

    //            var rows = deliverable.GanttTasks;
    //            var itemswithdependants = flatlist.Where(x => x.Dependants.Any()).ToList();
    //            for (int i = 0; i < itemswithdependants.Count; i++)
    //            {
    //                var dataitem = itemswithdependants[i];
    //                for (int j = 0; j < dataitem.Dependants.Count; j++)
    //                {
    //                    var item = dataitem.Dependants[j];
    //                    var dependant = rows.SingleOrDefault(x => x.Id == item.Id);
    //                    if (dependant != null)
    //                    {
    //                        dependant.DependentantId = dataitem.Id;
    //                        await repository.UpdateAsync(dependant);
    //                    }
    //                }
    //            }

    //        }

    //        async Task UpdateMapped(IRepository repository, Deliverable deliverable, List<GanttTaskResponse> flatlist)
    //        {
    //            var rows = deliverable.GanttTasks;
    //            for (int i = 0; i < flatlist.Count; i++)
    //            {
    //                var flat = flatlist[i];
    //                var row = rows.FirstOrDefault(x => x.Id == flat.Id);
    //                if (row == null)
    //                {
    //                    row = GanttTask.Create(deliverable.Id);
    //                    await repository.AddAsync(row);
    //                }
    //                else
    //                {
    //                    await repository.UpdateAsync(row);
    //                }
    //                flat.Map(row);
    //            }

    //        }


    //    }
    //    public static GanttTask Map(this GanttTaskResponse flat, GanttTask row)
    //    {

    //        row.Order = flat.Order;
    //        row.WBS = flat.WBS;
    //        row.Name = flat.Name;
    //        row.PlannedStartDate = flat.StartDate;
    //        row.PlannedEndDate = flat.EndDate;
    //        row.Duration = flat.Duration;
    //        row.Lag = flat.Lag;
    //        row.DependencyType = flat.DependencyType.Name;
    //        row.ParentId = flat.ParentGanttTaskId;
    //        row.LabelOrder = flat.LabelOrder;
    //        row.DependentantId = null;
    //        row.IsExpanded = flat.IsExpanded;
     
    //        return row;
    //    }
    //}


}
