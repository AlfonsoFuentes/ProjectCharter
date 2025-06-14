using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Valves.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Valves.Responses
{
    public class ValveResponse : BudgetItemWithPurchaseOrdersResponse, IMessageResponse, IRequest
    {


        public string EndPointName => StaticClass.Valves.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Valves.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
      
        public Guid? GanttTaskId { get; set; }
        public  List<BasicValveResponse> Items { get; set; } = new();

        public override List<BasicResponse> BasicEngineeringItems => Items.Count == 0 ? new() : Items.Select(x => new BasicResponse()
        {
            Id = x.Id,
            Name = x.Name,
            BudgetUSD = x.BudgetUSD,

        }).ToList();


    }
}
