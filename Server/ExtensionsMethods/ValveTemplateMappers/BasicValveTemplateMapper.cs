using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.ValvesEnum;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.Templates.Valves.Responses;

namespace Server.ExtensionsMethods.ValveTemplateMappers
{
    public static class BasicValveTemplateMapper
    {
        public static ValveTemplate Map(this ValveTemplateResponse request, ValveTemplate row)
        {
            row.Value = request.Value;
            row.TagLetter = request.TagLetter;

            row.Model = request.Model;
            row.BrandTemplateId = request.Brand!.Id;

            row.Type = request.Type.Id;
            row.HasFeedBack = request.HasFeedBack;
            row.Diameter = request.Diameter.Id;
            row.ActuatorType = request.ActuatorType.Id;
            row.FailType = request.FailType.Id;
            row.Material = request.Material.Id;
            row.PositionerType = request.PositionerType.Id;
            row.SignalType = request.SignalType.Id;
            row.ConnectionType = request.ConnectionType.Id;

            return row;
        }
        public static ValveTemplateResponse Map(this ValveTemplate row)
        {
            return new()
            {
                Id = row.Id,
                Brand = row.BrandTemplate == null ? new() : row.BrandTemplate.Map(),
                Material = MaterialEnum.GetType(row.Material),
                Model = row.Model,
                ActuatorType = ActuatorTypeEnum.GetType(row.ActuatorType),
                Type = ValveTypesEnum.GetType(row.Type),
                Diameter = NominalDiameterEnum.GetType(row.Diameter),
                FailType = FailTypeEnum.GetType(row.FailType),
                HasFeedBack = row.HasFeedBack,
                SignalType = SignalTypeEnum.GetType(row.SignalType),
                TagLetter = row.TagLetter,
                PositionerType = PositionerTypeEnum.GetType(row.PositionerType),

                ConnectionType = ConnectionTypeEnum.GetType(row.ConnectionType),
                Value = row.Value,
                Nozzles = row.NozzleTemplates.Count == 0 ? new() : row.NozzleTemplates.Select(x => x.Map()).ToList(),

            };
        }

        public static ValveTemplate Map(this BasicValveResponse request, ValveTemplate row)
        {

            row.Value = request.BudgetUSD;
            row.TagLetter = request.TagLetter;

            row.Model = request.Template.Model;
            row.BrandTemplateId = request.Template.Brand!.Id;

            row.Type = request.Template.Type.Id;
            row.HasFeedBack = request.Template.HasFeedBack;
            row.Diameter = request.Template.Diameter.Id;
            row.ActuatorType = request.Template.ActuatorType.Id;
            row.FailType = request.Template.FailType.Id;
            row.Material = request.Template.Material.Id;
            row.PositionerType = request.Template.PositionerType.Id;
            row.SignalType = request.Template.SignalType.Id;
            row.ConnectionType = request.Template.ConnectionType.Id;

            return row;
        }
        public static BasicValveItem Map(this BasicValveResponse request, BasicValveItem row)
        {
            row.Name = request.Name;
            row.TagLetter = request.TagLetter;
            row.TagNumber = request.TagNumber;
            row.IsExisting = request.IsExisting;
            row.BudgetUSD = request.BudgetUSD;
            row.ProvisionalTag = request.ProvisionalTag;
            return row;
        }
        public static async Task<ValveTemplate> AddValveTemplate(IRepository Repository, BasicValveResponse Data)
        {


            var valveTemplate = Template.AddValveTemplate();
            Data.Map(valveTemplate);
            foreach (var nozzle in Data.Nozzles)
            {
                var nozzleTemplate = NozzleTemplate.Create(valveTemplate.Id);
                nozzle.Map(nozzleTemplate);
                await Repository.AddAsync(nozzleTemplate);
            }
            await Repository.AddAsync(valveTemplate);
            return valveTemplate;

        }
        public static BasicValveResponse Map(this BasicValveItem row)
        {
            BasicValveResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,

                ProjectId = row.ProjectId,
                ValveId = row.ValveId!.Value,
                BudgetUSD = row.BudgetUSD,

                TagNumber = row.TagNumber,
                Template = row.ValveTemplate == null ? new() : row.ValveTemplate.Map(),
                Brand = row.ValveTemplate == null ? string.Empty : row.ValveTemplate.BrandName,
                TagLetter = row.TagLetter,
                ShowDetails = row.ValveTemplate != null,
                Nozzles = row.Nozzles == null || row.Nozzles.Count == 0 ? new() : row.Nozzles.Select(x => x.Map()).ToList(),
                IsExisting = row.IsExisting,
                ProvisionalTag = row.ProvisionalTag,
                ShowProvisionalTag = !string.IsNullOrWhiteSpace(row.ProvisionalTag),

            };

            return result;
        }
    }
}
