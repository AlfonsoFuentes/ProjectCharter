﻿using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Requests;

namespace Server.EndPoint.BudgetItems.BasicEngineerings.BasicValves.Commands
{
    public static class DeleteGroupBasicValveEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.BasicValves.EndPoint.DeleteGroup, async (DeleteGroupBasicValveRequest Data, IRepository Repository) =>
                {
                    foreach (var rowItem in Data.SelecteItems)
                    {
                        var row = await Repository.GetByIdAsync<BasicEquipmentItem>(rowItem.Id);
                        if (row != null)
                        {
                            await Repository.RemoveAsync(row);
                        }
                    }


                    var cache = StaticClass.BudgetItems.Cache.GetAll(Data.ProjectId);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache);
                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);

                });
            }
        }




    }

}
