using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.ExpendingTools.Records
{
    public class GetAllMonitoringExpendingTool : IGetAll
    {
        public string EndPointName => StaticClass.MonitoringExpendingTools.EndPoint.GetAll;
        public string Legend => "Get All Expending tool";
        public string ClassName => StaticClass.MonitoringExpendingTools.ClassName;
        public string ActionType => "GetAll";
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid ProjectId { get; set; }
    }
}
