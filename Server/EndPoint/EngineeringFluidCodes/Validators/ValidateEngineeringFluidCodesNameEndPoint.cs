using Shared.Models.EngineeringFluidCodes.Validators;

namespace Server.EndPoint.EngineeringFluidCodes.Validators
{
    public static class ValidateEngineeringFluidCodesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.Validate, async (ValidateEngineeringFluidCodeRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<EngineeringFluidCode, bool>> CriteriaId = null!;
                    Func<EngineeringFluidCode, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) && x.Code.Equals(Data.Code) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name) && x.Code.Equals(Data.Code);
                    string CacheKey = StaticClass.EngineeringFluidCodes.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }

}
