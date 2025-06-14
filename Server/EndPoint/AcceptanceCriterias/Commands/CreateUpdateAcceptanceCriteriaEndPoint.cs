using Shared.Models.AcceptanceCriterias.Responses;

namespace Server.EndPoint.AcceptanceCriterias.Commands
{

    public static class CreateUpdateAcceptanceCriteriaEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.AcceptanceCriterias.EndPoint.CreateUpdate, async (AcceptanceCriteriaResponse Data, IRepository Repository) =>
                {
                    AcceptanceCriteria? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        var lastorder = await Repository.GetLastOrderAsync<AcceptanceCriteria, Project>(Data.ProjectId);

                        row = AcceptanceCriteria.Create(Data.ProjectId, lastorder);

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<AcceptanceCriteria>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.AcceptanceCriterias.Cache.Key(row.Id, row.ProjectId)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static AcceptanceCriteria Map(this AcceptanceCriteriaResponse request, AcceptanceCriteria row)
        {
            row.Name = request.Name;
            return row;
        }

    }

}
