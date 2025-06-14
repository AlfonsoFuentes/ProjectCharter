using Shared.ExtensionsMetods;
using Shared.Models.EngineeringFluidCodes.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Pipings.Responses;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses
{
    public class BasicPipeResponse : BasicResponse, IMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid PipeId { get; set; }

        public string EndPointName => StaticClass.BasicPipes.EndPoint.CreateUpdate;

        public string Legend => Name;
        public PipeTemplateResponse Template { get; set; } = new();
        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.BasicPipes.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public bool ShowDetails { get; set; }
        public double BudgetCalculated => MaterialQuantity * EquivalentLenghPrice + LaborDayPrice * LaborQuantity;
        public override string TagNumber { get; set; } = string.Empty;
        public override string Tag =>  $"{Template.Diameter.Name}-{FluidCodeCode}-{TagNumber}-{Template.Material.Name}-{InsulationCode}";
        public string sBudgetUSD => BudgetUSD.ToCurrencyCulture();
   
        double materialQuantity;

        double laborQuantity;
        public double MaterialQuantity
        {
            get
            {
                return materialQuantity;
            }
            set
            {
                materialQuantity = value;
                BudgetUSD = BudgetCalculated;

            }
        }
        public double LaborDayPrice
        {
            get { return Template.LaborDayPrice; }
            set
            {
                Template.LaborDayPrice = value;
                BudgetUSD = BudgetCalculated;
            }
        }
        public double EquivalentLenghPrice
        {
            get { return Template.EquivalentLenghPrice; }
            set
            {
                Template.EquivalentLenghPrice = value;
                BudgetUSD = BudgetCalculated;
            }
        }
        public double LaborQuantity
        {
            get { return laborQuantity; }
            set
            {
                laborQuantity = value;
                BudgetUSD = BudgetCalculated;
            }
        }

        public EngineeringFluidCodeResponse? FluidCode { get; set; }
        public string FluidCodeCode => FluidCode != null ? FluidCode.Code : string.Empty;


        public string InsulationCode => Template.Insulation ? "1" : "0";

        public bool IsExisting { get; set; }



    }
}
