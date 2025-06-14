using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.Materials;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.Templates.Equipments.Responses;
namespace Server.ExtensionsMethods.EquipmentTemplateMapper
{
    public static class BasicEquipmentTemplateMapper
    {
        public static BasicEquipmentResponse Map(this BasicEquipmentItem row)
        {

            return new()
            {
                Id = row.Id,
                Name = row.Name,

                ProjectId = row.ProjectId,
                EquipmentId = row.EquipmentId!.Value,
                Template = row.EquipmentTemplate == null ? new() : row.EquipmentTemplate.Map(),
                Brand = row.EquipmentTemplate == null ? string.Empty : row.EquipmentTemplate.BrandName,
                TagNumber = row.TagNumber,

                ShowDetails = row.EquipmentTemplate == null ? false : true,
                Nozzles = row.Nozzles == null || row.Nozzles.Count == 0 ? new() : row.Nozzles.Select(x => x.Map()).ToList(),
                IsExisting = row.IsExisting,
                ProvisionalTag = row.ProvisionalTag,
                ShowProvisionalTag = !string.IsNullOrWhiteSpace(row.ProvisionalTag),
                BudgetUSD = row.BudgetUSD,

            };


        }
        public static BasicEquipmentItem Map(this BasicEquipmentResponse request, BasicEquipmentItem row)
        {
            row.Name = request.Name;
            row.TagLetter = request.ShowDetails || request.Template != null ? request.Template.TagLetter : string.Empty;
            row.TagNumber = request.TagNumber;
            row.IsExisting = request.IsExisting;
            row.BudgetUSD = request.BudgetUSD;
            row.ProvisionalTag = request.ProvisionalTag;
            return row;
        }
        public static EquipmentTemplate Map(this EquipmentTemplateResponse request, EquipmentTemplate row)
        {

            row.Value = request.Value;
            row.TagLetter = request.TagLetter;
            row.Reference = request.Reference;
            row.InternalMaterial = request.InternalMaterial.Id;
            row.ExternalMaterial = request.ExternalMaterial.Id;
            row.Model = request.Model;
            row.BrandTemplateId = request.Brand!.Id;
            row.SubType = request.SubType;
            row.Type = request.Type;

            return row;
        }
        public static EquipmentTemplateResponse Map(this EquipmentTemplate row)
        {
            return new()
            {
                Value = row.Value,
                Id = row.Id,
                TagLetter = row.TagLetter,
                Reference = row.Reference,
                Brand = row.BrandTemplate == null ? null : new BrandResponse()
                {
                    Id = row.BrandTemplate.Id,
                    Name = row.BrandTemplate.Name
                },
                ExternalMaterial = MaterialEnum.GetType(row.ExternalMaterial),
                InternalMaterial = MaterialEnum.GetType(row.InternalMaterial),
                Model = row.Model,
                Name = $"{row.Type} {row.Model}",
                Type = row.Type,
                SubType = row.SubType,
                Nozzles = row.NozzleTemplates.Select(x => x.Map()).ToList(),
            };


        }


        public static async Task<EquipmentTemplate> AddEquipmentTemplate(IRepository Repository, BasicEquipmentResponse Data)
        {
            var equipmentTemplate = Template.AddEquipmentTemplate();
            Data.Template.Map(equipmentTemplate);
            equipmentTemplate.Value = Data.BudgetUSD;


            foreach (var nozzle in Data.Nozzles)
            {
                var nozzleTemplate = NozzleTemplate.Create(equipmentTemplate.Id);
                nozzle.Map(nozzleTemplate);
                await Repository.AddAsync(nozzleTemplate);
            }
            await Repository.AddAsync(equipmentTemplate);
            return equipmentTemplate;

        }

    }
}
