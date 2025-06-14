using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;

namespace Server.EndPoint.BudgetItems.Queries
{
    public static class GetAllBudgetItemForAssignGanntTaskEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BudgetItems.EndPoint.GetAllAssignGantTask, async (BudgetItemGetAllAssignGanttTask request, IQueryRepository repository) =>
                {
                    var row = await GetBudgetItemAsync(request, repository);

                    if (row == null)
                    {
                        return Result<BudgetItemResponseAssignGanttTaskList>.Fail(
                            StaticClass.ResponseMessages.ReponseNotFound(StaticClass.BudgetItems.ClassLegend));
                    }
                    if (row.BudgetItems == null || row.BudgetItems.Count == 0)
                    {
                        return Result<BudgetItemResponseAssignGanttTaskList>.Success(new BudgetItemResponseAssignGanttTaskList
                        {
                            Items = new List<BudgetItemResponseAssignGanttTask>()
                        });
                    }
                    var Alterations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Alteration>().Select(x => x.MapLocal()).ToList();

                    var Structurals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Structural>().Select(x => x.MapLocal()).ToList();
                    var Foundations = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Foundation>().Select(x => x.MapLocal()).ToList();
                    
                    var Equipments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.GetEquipmentList();
                    var Valves = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.GetValvesList();
                    var Electricals = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Electrical>().Select(x => x.MapLocal()).ToList();
                    var Pipings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.GetPipeList();
                    var Instruments = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.GetInstrumentList();
                    var EHSs = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<EHS>().Select(x => x.MapLocal()).ToList();
                    var Paintings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Painting>().Select(x => x.MapLocal()).ToList();
                    var Taxes = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Tax>().Select(x => x.MapLocal()).ToList();
                    var Testings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Testing>().Select(x => x.MapLocal()).ToList();

                    var EngineeringDesigns = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<EngineeringDesign>().Select(x => x.MapLocal()).ToList();
                    var Engineerings = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Engineering>().Select(x => x.MapLocal()).ToList();
                    var Contingencys = row.BudgetItems == null || row.BudgetItems.Count == 0 ? new() : row.BudgetItems.OfType<Contingency>().Select(x => x.MapLocal()).ToList();

                    List<BudgetItemResponseAssignGanttTask> items = [.. Alterations, .. Structurals, .. Foundations, .. Equipments, .. Valves, .. Electricals,
                        .. Pipings, .. Instruments, .. EHSs, .. Paintings, .. Taxes, .. Testings, .. EngineeringDesigns,
                        .. Engineerings, .. Contingencys];


                    BudgetItemResponseAssignGanttTaskList response = new()
                    {


                        Items = items,


                    };


                    return Result<BudgetItemResponseAssignGanttTaskList>.Success(response);
                });
            }

            private static async Task<Project?> GetBudgetItemAsync(BudgetItemGetAllAssignGanttTask request, IQueryRepository repository)
            {
                Func<IQueryable<Project>, IIncludableQueryable<Project, object>> includes = x => x
                .Include(p => p.BudgetItems)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!).ThenInclude(x => x.BrandTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.FluidCode)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!).ThenInclude(x => x.BrandTemplate)
                .Include(x => x.BudgetItems).ThenInclude(x => x.BudgetItemNewGanttTasks)
                ;

                Expression<Func<Project, bool>> criteria = x => x.Id == request.ProjectId;
                string cacheKey = StaticClass.BudgetItems.Cache.GetAll(request.ProjectId);

                return await repository.GetAsync(Cache: cacheKey, Includes: includes, Criteria: criteria);
            }


        }
        static BudgetItemResponseAssignGanttTask MapLocal(this BudgetItem item)
        {
            return new BudgetItemResponseAssignGanttTask
            {
                Id = item.Id,
                Name = item.Name,
                BudgetUSD = item.BudgetUSD,
                Nomenclatore = item.Nomenclatore,
                Order = item.Order,

            };
        }
        static List<BudgetItemResponseAssignGanttTask> GetEquipmentList(this Project row)
        {

            List<BudgetItemResponseAssignGanttTask> Equipments = new();
            if (row.BudgetItems!.OfType<Equipment>().Count() > 0)
            {
                var equipments = row.BudgetItems!.OfType<Equipment>().ToList();
                foreach (var equipment in equipments)
                {
                    if (equipment.EquipmentItems == null || equipment.EquipmentItems.Count == 0)
                    {
                        BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                        {
                            Id = equipment.Id,
                            Name = equipment.Name,
                            BudgetUSD = equipment.BudgetUSD,
                            Nomenclatore = equipment.Nomenclatore,
                            Order = equipment.OrderList
                        };
                        Equipments.Add(budgetItemResponseAssignGanttTask);
                    }
                    else
                    {
                        foreach (var item in equipment.EquipmentItems)
                        {
                            BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                            {
                                Id = equipment.Id,
                                Name = equipment.Name,
                                BudgetUSD = equipment.BudgetUSD,
                                Nomenclatore = equipment.Nomenclatore,
                                Order = equipment.OrderList,
                                SubId = item.Id,
                                TagNumber = item.TagNumber,
                            };
                            Equipments.Add(budgetItemResponseAssignGanttTask);
                        }
                    }

                }
            }
            return Equipments;
        }
        static List<BudgetItemResponseAssignGanttTask> GetValvesList(this Project row)
        {

            List<BudgetItemResponseAssignGanttTask> Equipments = new();
            if (row.BudgetItems!.OfType<Equipment>().Count() > 0)
            {
                var equipments = row.BudgetItems!.OfType<Valve>().ToList();
                foreach (var equipment in equipments)
                {
                    if (equipment.ValveItems == null || equipment.ValveItems.Count == 0)
                    {
                        BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                        {
                            Id = equipment.Id,
                            Name = equipment.Name,
                            BudgetUSD = equipment.BudgetUSD,
                            Nomenclatore = equipment.Nomenclatore,
                            Order = equipment.OrderList
                        };
                        Equipments.Add(budgetItemResponseAssignGanttTask);
                    }
                    else
                    {
                        foreach (var item in equipment.ValveItems)
                        {
                            BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                            {
                                Id = equipment.Id,
                                Name = equipment.Name,
                                BudgetUSD = equipment.BudgetUSD,
                                Nomenclatore = equipment.Nomenclatore,
                                Order = equipment.OrderList,
                                SubId = item.Id,
                                TagNumber = item.TagNumber,
                            };
                            Equipments.Add(budgetItemResponseAssignGanttTask);
                        }
                    }

                }
            }
            return Equipments;
        }
        static List<BudgetItemResponseAssignGanttTask> GetInstrumentList(this Project row)
        {

            List<BudgetItemResponseAssignGanttTask> Equipments = new();
            if (row.BudgetItems!.OfType<Instrument>().Count() > 0)
            {
                var equipments = row.BudgetItems!.OfType<Instrument>().ToList();
                foreach (var equipment in equipments)
                {
                    if (equipment.InstrumentItems == null || equipment.InstrumentItems.Count == 0)
                    {
                        BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                        {
                            Id = equipment.Id,
                            Name = equipment.Name,
                            BudgetUSD = equipment.BudgetUSD,
                            Nomenclatore = equipment.Nomenclatore,
                            Order = equipment.OrderList
                        };
                        Equipments.Add(budgetItemResponseAssignGanttTask);
                    }
                    else
                    {
                        foreach (var item in equipment.InstrumentItems)
                        {
                            BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                            {
                                Id = equipment.Id,
                                Name = equipment.Name,
                                BudgetUSD = equipment.BudgetUSD,
                                Nomenclatore = equipment.Nomenclatore,
                                Order = equipment.OrderList,
                                SubId = item.Id,
                                TagNumber = item.TagNumber,
                            };
                            Equipments.Add(budgetItemResponseAssignGanttTask);
                        }
                    }

                }
            }
            return Equipments;
        }
        static List<BudgetItemResponseAssignGanttTask> GetPipeList(this Project row)
        {

            List<BudgetItemResponseAssignGanttTask> Equipments = new();
            if (row.BudgetItems!.OfType<Pipe>().Count() > 0)
            {
                var equipments = row.BudgetItems!.OfType<Pipe>().ToList();
                foreach (var equipment in equipments)
                {
                    if (equipment.PipeItems == null || equipment.PipeItems.Count == 0)
                    {
                        BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                        {
                            Id = equipment.Id,
                            Name = equipment.Name,
                            BudgetUSD = equipment.BudgetUSD,
                            Nomenclatore = equipment.Nomenclatore,
                            Order = equipment.OrderList
                        };
                        Equipments.Add(budgetItemResponseAssignGanttTask);
                    }
                    else
                    {
                        foreach (var item in equipment.PipeItems)
                        {
                            BudgetItemResponseAssignGanttTask budgetItemResponseAssignGanttTask = new()
                            {
                                Id = equipment.Id,
                                Name =$"{equipment.Name} - {item.Name}" ,
                                BudgetUSD = equipment.BudgetUSD,
                                Nomenclatore = equipment.Nomenclatore,
                                Order = equipment.OrderList,
                                SubId = item.Id,
                                TagNumber = item.TagNumber,
                            };
                            Equipments.Add(budgetItemResponseAssignGanttTask);
                        }
                    }

                }
            }
            return Equipments;
        }

    }
}