using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Responses
{
    public class EngineeringFluidCodeResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.CreateUpdate;
        public string Legend => Name;
        public string Code { get; set; } = string.Empty;
        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.EngineeringFluidCodes.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
    }
}
