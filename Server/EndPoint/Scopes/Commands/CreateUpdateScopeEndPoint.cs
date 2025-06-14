using Server.Database.Entities.ProjectManagements;
using Shared.Models.Scopes.Requests;

namespace Server.EndPoint.Scopes.Commands
{

    public static class CreateUpdateScopeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Scopes.EndPoint.CreateUpdate, async (ScopeResponse Data, IRepository Repository) =>
                {
                    Scope? row = null!;
                    if(Data.Id==Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<Scope, Project>(Data.ProjectId);

                        row = Scope.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Scope>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }

                   

                    Data.Map(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(GetCacheKeys(row));


                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);
                });


            }


            private string[] GetCacheKeys(Scope row)
            {
                List<string> cacheKeys = [

                    
                    .. StaticClass.Scopes.Cache.Key(row.Id, row.ProjectId)
                ];
                return cacheKeys.Where(key => !string.IsNullOrEmpty(key)).ToArray();
            }
        }


        static Scope Map(this ScopeResponse request, Scope row)
        {
            row.Name = request.Name;
  
            return row;
        }

    }

}
