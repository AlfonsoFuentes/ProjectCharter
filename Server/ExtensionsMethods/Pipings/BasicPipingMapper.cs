using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.EndPoint.Brands.Queries;
using Server.EndPoint.EngineeringFluidCodes.Queries;
using Server.EndPoint.PurchaseOrders.Queries;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace Server.ExtensionsMethods.Pipings
{
    public static class BasicPipingMapper
    {
        public static PipeTemplate Map(this PipeTemplateResponse request, PipeTemplate row)
        {

            row.Diameter = request.Diameter.Id;
            row.Class = request.Class.Id;
            row.EquivalentLenghPrice = request.EquivalentLenghPrice;
            row.LaborDayPrice = request.LaborDayPrice;
            row.Insulation = request.Insulation;
            row.Material = request.Material.Id;


            return row;
        }
        public static PipeTemplate Map(this BasicPipeResponse request, PipeTemplate row)
        {

            row.Diameter = request.Template.Diameter.Id;
            row.Class = request.Template.Class.Id;
            row.EquivalentLenghPrice = request.EquivalentLenghPrice;
            row.LaborDayPrice = request.LaborDayPrice;
            row.Insulation = request.Template.Insulation;
            row.Material = request.Template.Material.Id;


            return row;
        }
        public static PipeTemplateResponse Map(this PipeTemplate row)
        {
            return new()
            {
                Id = row.Id,

                Material = MaterialEnum.GetType(row.Material),
                Class = PipeClassEnum.GetType(row.Class),
                Diameter = NominalDiameterEnum.GetType(row.Diameter),
                EquivalentLenghPrice = row.EquivalentLenghPrice,
                Insulation = row.Insulation,
                LaborDayPrice = row.LaborDayPrice,




            };
        }

        public static BasicPipeItem Map(this BasicPipeResponse request, BasicPipeItem row)
        {
            row.Name = request.Name;
            row.BudgetUSD = request.BudgetUSD;
            row.BasicFluidCodeId = request.FluidCode == null ? null : request.FluidCode.Id;
            row.LaborQuantity = request.LaborQuantity;
            row.MaterialQuantity = request.MaterialQuantity;
            row.TagNumber = request.TagNumber;

            row.IsExisting = request.IsExisting;

            return row;
        }
        public static BasicPipeResponse Map(this BasicPipeItem row)
        {
            BasicPipeResponse result = new()
            {
                Id = row.Id,
                Name = row.Name,
              
                ProjectId = row.ProjectId,

                PipeId = row.PipeId!.Value,
                TagNumber = row.TagNumber,

                ShowDetails = row.PipeTemplate != null,
                Template = row.PipeTemplate == null ? new() : row.PipeTemplate.Map(),


                LaborQuantity = row.LaborQuantity,
                MaterialQuantity = row.MaterialQuantity,
                FluidCode = row.FluidCode == null ? null : row.FluidCode.Map(),
                BudgetUSD = row.BudgetUSD,
                IsExisting = row.IsExisting,
                
            };
            return result;

        }
        public static async Task<PipeTemplate> AddPipeTemplate(IRepository Repository, BasicPipeResponse Data)
        {

            var pipeTemplate = Template.AddPipeTemplate();
            Data.Map(pipeTemplate);
            await Repository.AddAsync(pipeTemplate);


            return pipeTemplate;

        }
    }
}
