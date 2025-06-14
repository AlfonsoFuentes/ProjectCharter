using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Instruments.Responses
{
    public class InstrumentResponse : BudgetItemWithPurchaseOrdersResponse, IMessageResponse, IRequest
    {

        public Guid? TemplateId { get; set; }
        public string EndPointName => StaticClass.Instruments.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Instruments.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid? GanttTaskId { get; set; }

        public   List<BasicInstrumentResponse> Items { get; set; } = new();

        public override  List<BasicResponse> BasicEngineeringItems => Items.Count == 0 ? new() : Items.Select(x => new BasicResponse()
        {
            Id = x.Id,
            Name = x.Name,
            BudgetUSD = x.BudgetUSD,

        }).ToList();
    }
}
