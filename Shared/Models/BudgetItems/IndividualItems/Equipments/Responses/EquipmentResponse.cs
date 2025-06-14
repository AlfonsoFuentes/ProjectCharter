using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Equipments.Responses
{
    public class EquipmentResponse : BudgetItemWithPurchaseOrdersResponse, IMessageResponse, IRequest
    {

        public string EndPointName => StaticClass.Equipments.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Equipments.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid? GanttTaskId { get; set; }    
        public  List<BasicEquipmentResponse> Items { get; set; } = new();

        public override List<BasicResponse> BasicEngineeringItems => Items.Count == 0 ? new() : Items.Select(x => new BasicResponse()
        {
            Id = x.Id,
            Name = x.Name,
            BudgetUSD = x.BudgetUSD,

        }).ToList();
    }
}
