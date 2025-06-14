using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.ExtensionsMethods.InstrumentTemplateMapper;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Instruments;
using Shared.Enums.Materials;
using Shared.Enums.ValvesEnum;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.Templates.Instruments.Responses;

namespace Server.ExtensionsMethods.InstrumentTemplateMapper
{
    public static class BasicInstrumentTemplateMapper
    {
        public static BasicInstrumentResponse Map(this BasicInstrumentItem row)
        {

            return new()
            {
                Id = row.Id,
                Name = row.Name,

                ProjectId = row.ProjectId,
                InstrumentId = row.InstrumentId!.Value,
                Template = row.InstrumentTemplate == null ? new() : row.InstrumentTemplate.Map(),
                Brand = row.InstrumentTemplate == null ? string.Empty : row.InstrumentTemplate.BrandName,
                TagNumber = row.TagNumber,

                ShowDetails = row.InstrumentTemplate == null ? false : true,
                Nozzles = row.Nozzles == null || row.Nozzles.Count == 0 ? new() : row.Nozzles.Select(x => x.Map()).ToList(),
                IsExisting = row.IsExisting,
                ProvisionalTag = row.ProvisionalTag,
                ShowProvisionalTag = !string.IsNullOrWhiteSpace(row.ProvisionalTag),
                BudgetUSD = row.BudgetUSD,

            };


        }
        public static InstrumentTemplate Map(this InstrumentTemplateResponse request, InstrumentTemplate row)
        {
            row.Value = request.Value;
            row.TagLetter = request.TagLetter;
            row.Reference = request.Reference;
            row.Material = request.Material.Id;

            row.Model = request.Model;
            row.BrandTemplateId = request.Brand!.Id;
            row.ModifierVariable = request.ModifierVariable.Id;
            row.Variable = request.VariableInstrument.Id;
            row.SignalType = request.SignalType.Id;

            row.Diameter = request.Diameter.Id;
            row.ConnectionType = request.ConnectionType.Id;

            return row;
        }
        public static InstrumentTemplateResponse Map(this InstrumentTemplate row)
        {
            return new()
            {
                Id = row.Id,
                Brand = row.BrandTemplate == null ? new() : row.BrandTemplate.Map(),
                Material = MaterialEnum.GetType(row.Material),

                Model = row.Model,
                Reference = row.Reference,
                ModifierVariable = ModifierVariableInstrumentEnum.GetType(row.ModifierVariable),

                VariableInstrument = VariableInstrumentEnum.GetType(row.Variable),
                Value = row.Value,
                SignalType = SignalTypeEnum.GetType(row.SignalType),
                Nozzles = row.NozzleTemplates.Count == 0 ? new() : row.NozzleTemplates.Select(x => x.Map()).ToList(),

                Diameter = NominalDiameterEnum.GetType(row.Diameter),
                ConnectionType = ConnectionTypeEnum.GetType(row.ConnectionType),

            };
        }


        public static InstrumentTemplate Map(this BasicInstrumentResponse request, InstrumentTemplate row, double Value)
        {

            row.Value = Value;

            row.TagLetter = request.TagLetter;
            row.Reference = request.Template.Reference;
            row.Material = request.Template.Material.Id;

            row.Model = request.Template.Model;
            row.BrandTemplateId = request.Template.Brand!.Id;
            row.ModifierVariable = request.Template.ModifierVariable.Id;
            row.Variable = request.Template.VariableInstrument.Id;
            row.SignalType = request.Template.SignalType.Id;

            row.Diameter = request.Template.Diameter.Id;
            row.ConnectionType = request.Template.ConnectionType.Id;

            return row;
        }
        public static BasicInstrumentItem Map(this BasicInstrumentResponse request, BasicInstrumentItem row)
        {
            row.Name = request.Name;
            row.TagLetter = request.TagLetter;
            row.TagNumber = request.TagNumber;
            row.IsExisting = request.IsExisting;
            row.BudgetUSD = request.BudgetUSD;
            row.ProvisionalTag = request.ProvisionalTag;
            return row;
        }
        public static async Task<InstrumentTemplate> AddInstrumentTemplate(IRepository Repository, BasicInstrumentResponse data)
        {
            var instrumentTemplate = Template.AddInstrumentTemplate();
            data.Map(instrumentTemplate, data.BudgetUSD);
            foreach (var nozzle in data.Nozzles)
            {
                var nozzleTemplate = NozzleTemplate.Create(instrumentTemplate.Id);
                nozzle.Map(nozzleTemplate);
                await Repository.AddAsync(nozzleTemplate);
            }
            await Repository.AddAsync(instrumentTemplate);
            return instrumentTemplate;

        }

    }
}
