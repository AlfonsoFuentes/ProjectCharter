using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Brands.Responses
{
    public class BrandResponse : BaseResponse, IMessageResponse, IRequest
    {


        public string EndPointName => StaticClass.Brands.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Brands.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
    }
}
