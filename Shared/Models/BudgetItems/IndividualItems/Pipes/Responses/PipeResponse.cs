using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.EngineeringFluidCodes.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Pipings.Responses;

namespace Shared.Models.BudgetItems.IndividualItems.Pipes.Responses
{
    public class PipeResponse : BudgetItemWithPurchaseOrdersResponse, IMessageResponse, IRequest
    {

        public string EndPointName => StaticClass.Pipes.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Pipes.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid? GanttTaskId { get; set; }
       
        public List<BasicPipeResponse> Items { get; set; } = new();
        public override List<BasicResponse> BasicEngineeringItems => Items.Count==0 ? new() : Items.Select(x => new BasicResponse()
        {
            Id = x.Id,
            Name = x.Name,
            BudgetUSD = x.BudgetUSD,

        }).ToList();

    }
}
