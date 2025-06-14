using Server.EndPoint.EngineeringFluidCodes.Queries;
using Shared.Models.EngineeringFluidCodes.Responses;

namespace Server.EndPoint.EngineeringFluidCodes.Commands
{

    public static class CreateUpdateEngineeringFluidCodeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.CreateUpdate, async (EngineeringFluidCodeResponse Data, IRepository Repository) =>
                {
                    EngineeringFluidCode? row = null;   
                    if(Data.Id==Guid.Empty)
                    {
                        row = EngineeringFluidCode.Create();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<EngineeringFluidCode>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }
                  

                    Data.Map(row);
                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(StaticClass.EngineeringFluidCodes.Cache.Key(row.Id));

                    var response = row.Map();
                    return Result.EndPointResult(response,result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        static EngineeringFluidCode Map(this EngineeringFluidCodeResponse request, EngineeringFluidCode row)
        {
            row.Name = request.Name;
            row.Code = request.Code;
            return row;
        }

    }

}
