using Shared.ExtensionsMetods;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Instruments.Responses;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses
{
    public class BasicInstrumentResponse : BasicResponse, IMessageResponse, IRequest
    {

        public Guid? TemplateId { get; set; }
        public string EndPointName => StaticClass.BasicInstruments.EndPoint.CreateUpdate;
      
        public string sBudgetUSD => BudgetUSD.ToCurrencyCulture();
        public string Legend => Name;
        public string Brand { get; set; } = string.Empty;
        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.BasicInstruments.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid ProjectId { get; set; }
        public Guid InstrumentId { get; set; }


        public override string TagNumber { get; set; } = string.Empty;

        public InstrumentTemplateResponse Template { get; set; } = new();

        public List<NozzleResponse> Nozzles { get; set; } = new();
        public string TagLetter => Template.TagLetter;
        string ShowTagNumber => string.IsNullOrWhiteSpace(TagNumber) ? string.Empty : $"-{TagNumber}";
        public override string Tag => ShowProvisionalTag ? ProvisionalTag : $"{TagLetter}{ShowTagNumber}";
        public bool ShowDetails { get; set; }

        public bool IsExisting { get; set; }
        public string ProvisionalTag { get; set; } = string.Empty;
        public bool ShowProvisionalTag { get; set; } = false;



    }
}
