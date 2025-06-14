using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItemNewGanttTasks.Responses
{
    public class BudgetItemNewGanttTaskResponseList : IMessageResponse, IResponseAll, IRequest
    {
        public string EndPointName => StaticClass.BudgetItemNewGanttTasks.EndPoint.UpdateAll;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string Legend => "";

        public string ActionType => "updated";
        public string ClassName => StaticClass.BudgetItemNewGanttTasks.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public List<BudgetItemNewGanttTaskResponse> Items { get; set; } = new List<BudgetItemNewGanttTaskResponse>();
    }
    
}
