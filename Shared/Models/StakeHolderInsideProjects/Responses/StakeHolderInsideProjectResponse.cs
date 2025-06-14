using Shared.Enums.StakeHolderTypes;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.StakeHolders.Responses;

namespace Shared.Models.StakeHolderInsideProjects.Responses
{
    public class StakeHolderInsideProjectResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => Create ? StaticClass.StakeHolderInsideProjects.EndPoint.Create : StaticClass.StakeHolderInsideProjects.EndPoint.Update;

        public string Legend => Name;
        public bool Create { get; set; } = false;
        public string ActionType => Create ? "created" : "updated";
        public string ClassName => StaticClass.StakeHolderInsideProjects.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public StakeHolderResponse? StakeHolder { get; set; }
        public string? StakeHolderName => StakeHolder == null ? string.Empty : StakeHolder.Name;
        public Guid? OriginalStakeHolderId { get; set; }
        public Guid ProjectId { get; set; }
        public StakeHolderRoleEnum Role { get; set; } = StakeHolderRoleEnum.None;
    }
}
