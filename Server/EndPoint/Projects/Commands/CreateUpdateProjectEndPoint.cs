namespace Server.EndPoint.Projects.Commands
{

    public static class CreateUpdateProjectEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.CreateUpdate, async (ProjectResponse Data, IRepository Repository, IQueryRepository queryRepository) =>
                {
                    Project? row;
                    if (Data.Id == Guid.Empty)
                    {
                        var projects = await queryRepository.GetAllAsync<Project>(Cache: StaticClass.Projects.Cache.GetAll);

                        int lastorder = projects == null || projects.Count == 0 ? 1 : projects.MaxBy(x => x.Order)!.Order + 1;
                        row = Project.Create(lastorder);
                        await Repository.AddAsync(row);
                        var contingency = Contingency.Create(row.Id);
                        contingency.Order = 1;

                        var engineering = Engineering.Create(row.Id);
                        engineering.Order = 1;

                        engineering.Percentage = Data.PercentageEngineering;
                        contingency.Percentage = Data.PercentageContingency;

                        engineering.Name = $"Engineering {Data.PercentageEngineering}%";
                        contingency.Name = $"Contingency {Data.PercentageContingency}%";

                        await Repository.AddAsync(contingency);
                        await Repository.AddAsync(engineering);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Project>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(StaticClass.Projects.Cache.Key(row.Id));

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }

        }


        static Project Map(this ProjectResponse request, Project row)
        {
            row.Name = request.Name;
      

            row.StartDate = request.InitialProjectDate == null ? null : request.InitialProjectDate.Value;


            row.PercentageEngineering = request.PercentageEngineering;
            row.PercentageContingency = request.PercentageContingency;
            row.PercentageTaxProductive = request.PercentageTaxProductive;
            row.IsProductiveAsset = request.IsProductiveAsset;
            row.ProjectNeedType = request.ProjectNeedType.Id;
            row.CostCenter = request.CostCenter.Id;
            row.Focus = request.Focus.Id;
       
            return row;
        }

    }

}
