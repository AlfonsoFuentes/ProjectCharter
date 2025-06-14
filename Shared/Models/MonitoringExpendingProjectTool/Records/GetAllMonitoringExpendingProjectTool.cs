using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.MonitoringExpendingProjectTool.Records
{
    public class GetAllMonitoringExpendingProjectTool : IGetAll
    {
        public string EndPointName => StaticClass.MonitoringExpendingToolProjects.EndPoint.GetAll;
        public string Legend => "Get All Expending tool";
        public string ClassName => StaticClass.MonitoringExpendingToolProjects.ClassName;
        public string ActionType => "GetAll";
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
     
    }
}
