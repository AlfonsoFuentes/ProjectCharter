using Shared.Models.FileResults.Generics.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ExpendingTools.Records
{
    public class GetAllExpendingTool : IGetAll
    {
        public string EndPointName => StaticClass.ExpendingTools.EndPoint.GetAll;
        public string Legend => "Get All Expending tool";
        public string ClassName => StaticClass.ExpendingTools.ClassName;
        public string ActionType => "GetAll";
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid ProjectId { get; set; }
    }
}
