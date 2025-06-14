using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.ExpertJudgements.Responses
{
    public class ExpertJudgementResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.ExpertJudgements.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);


        public Guid ProjectId { get; set; }
        public StakeHolderResponse? Expert { get; set; }
        public string ExpertName => Expert == null ? string.Empty : Expert.Name;
    }
}
