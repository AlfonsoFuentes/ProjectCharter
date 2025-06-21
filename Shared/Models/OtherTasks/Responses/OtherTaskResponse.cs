using Shared.Enums.OtherTask;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Responses
{
    public class OtherTaskResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.OtherTasks.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.OtherTasks.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);


        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public string CECName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public string sStartDate => StartDate?.ToString("d") ?? string.Empty;
        public string sEndDate => EndDate?.ToString("d") ?? string.Empty;
    }
}
