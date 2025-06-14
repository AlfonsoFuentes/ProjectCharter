using Shared.ExtensionsMetods;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Valves.Responses;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses
{
    public class BasicValveResponse : BasicResponse, IMessageResponse, IRequest
    {

        public Guid ProjectId { get; set; }
        public Guid ValveId { get; set; }
        public string EndPointName => StaticClass.BasicValves.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.BasicValves.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public bool ShowDetails { get; set; }
        public ValveTemplateResponse Template { get; set; } = new();
        public override string TagNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string sBudgetUSD => BudgetUSD.ToCurrencyCulture();
        public override string Tag => ShowProvisionalTag ? ProvisionalTag :
            string.IsNullOrEmpty(TagLetter) ? string.Empty :
            string.IsNullOrEmpty(TagNumber) ? TagLetter : $"{TagLetter}-{TagNumber}";
        public string TagLetter { get; set; } = string.Empty;

        public List<NozzleResponse> Nozzles { get; set; } = new();

        public bool IsExisting { get; set; }
        public string ProvisionalTag { get; set; } = string.Empty;
        public bool ShowProvisionalTag { get; set; } = false;




    }
}
