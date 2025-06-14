using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Responses
{
    public class ResourceResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Resources.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Resources.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);



        public Guid ProjectId { get; set; }
    }
}
