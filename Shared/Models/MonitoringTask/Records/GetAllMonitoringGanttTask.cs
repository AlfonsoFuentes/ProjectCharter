using Shared.Models.FileResults.Generics.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MonitoringTask.Records
{
    public class GetAllMonitoringGanttTask : IGetAll
    {
        public string EndPointName => StaticClass.MonitoringTasks.EndPoint.GetAll;
        public string Legend => "Get All Tasks";
        public string ClassName => StaticClass.MonitoringTasks.ClassName;
        public string ActionType => "GetAll";
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid ProjectId { get; set; }
    }
}
