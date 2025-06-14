using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Equipments;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Nozzles;
using Server.ExtensionsMethods.EquipmentTemplateMapper;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Commons;
using Shared.Models.Templates.Instruments.Validators;
using Shared.Models.Templates.NozzleTemplates;
using static Shared.StaticClasses.StaticClass;

namespace Server.EndPoint.Templates.Instruments.Validators
{
    public static class ValidateInstrumentTemplatesNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.InstrumentTemplates.EndPoint.Validate, async (ValidateInstrumentTemplateRequest Data, IQueryRepository Repository) =>
                {
                    Func<IQueryable<InstrumentTemplate>, IIncludableQueryable<InstrumentTemplate, object>> Includes = x => x
                   
                    .Include(x => x.BrandTemplate!)
                    .Include(x => x.NozzleTemplates);

                    Func<InstrumentTemplate, bool> CriteriaInstrument = x =>
                    x.Material == Data.Material &&
                       x.SignalType == Data.SignalType &&
                       x.Variable == Data.VariableInstrument &&
                       x.ModifierVariable == Data.ModifierVariable &&
                        x.BrandName.Equals(Data.Brand, StringComparison.OrdinalIgnoreCase) &&
                        x.Model.Equals(Data.Model, StringComparison.OrdinalIgnoreCase)
                       ;


                    string CacheKey = StaticClass.InstrumentTemplates.Cache.GetAll;
                    var instrumentTemplates = await Repository.GetAllToValidateAsync(Cache: CacheKey, Includes: Includes, Criteria: CriteriaInstrument);



                    instrumentTemplates = Data.Id.HasValue ? instrumentTemplates.Where(x => x.Id != Data.Id.Value).ToList() : instrumentTemplates;
                    if (instrumentTemplates == null || !instrumentTemplates.Any())
                    {
                        return false;
                    }
                    // Validar las boquillas para cada template coincidente
                    foreach (var instrumentTemplate in instrumentTemplates)
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
