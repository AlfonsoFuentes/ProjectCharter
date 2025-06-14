using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Responses
{
    public class QualityResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Qualitys.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Qualitys.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);



        public Guid ProjectId { get; set; }
    }
}
