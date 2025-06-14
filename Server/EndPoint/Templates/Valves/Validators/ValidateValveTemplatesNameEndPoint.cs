using Shared.Models.Templates.Valves.Validators;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.Templates.Valves.Validators
{
    public static class ValidateValveTemplatesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.ValveTemplates.EndPoint.Validate, async (ValidateValveTemplateRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<ValveTemplate>, IIncludableQueryable<ValveTemplate, object>> Includes = x => x
              
                    .Include(x => x.BrandTemplate!)
                    .Include(x=>x.NozzleTemplates)
                     ;



                    Func<ValveTemplate, bool> CriteriaValve = x =>
                    x.Material==Data.Material &&
                    x.SignalType==Data.SignalType &&
                    x.FailType == Data.FailType &&
                    x.Type == Data.Type &&
                    x.ActuatorType == Data.ActuadorType &&
                    x.Diameter == Data.Diameter &&
                    x.HasFeedBack == Data.HasFeedBack &&
                    x.PositionerType == Data.PositionerType &&
                    x.BrandName.Equals(Data.Brand) &&
                    x.Model.Equals(Data.Model) 
              

                  
                    ;

                    string CacheKey = StaticClass.ValveTemplates.Cache.GetAll;

                    var valveTemplates = await Repository.GetAllToValidateAsync(Cache: CacheKey, Includes: Includes, Criteria: CriteriaValve);

                    valveTemplates = Data.Id.HasValue ? valveTemplates.Where(x => x.Id != Data.Id.Value).ToList() : valveTemplates;
                    if (valveTemplates == null || !valveTemplates.Any())
                    {
                        return false;
                    }
                    // Validar las boquillas para cada template coincidente
                    foreach (var instrumentTemplate in valveTemplates)
                    {
                        if (instrumentTemplate.NozzleTemplates.ValidateNozzles(Data.NozzleTemplates))
                        {
                            return true; // Si todas las boquillas coinciden, retornar true
                        }
                    }
                    return false;
                });


            }
        }



    }

}
