using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.FinishingLines.BackBones
{
    public class BackBoneResponse:BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Backbones.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Backbones.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public string Description { get; set; } = string.Empty;

    }
    public class DeleteBackBoneRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Backbones.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Backbones.EndPoint.Delete;
    }
    public class GetBackBoneByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Backbones.EndPoint.GetById;
        public override string ClassName => StaticClass.Backbones.ClassName;
    }
    public class BackBoneGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Backbones.EndPoint.GetAll;
    }
    public class BackBoneResponseList : IResponseAll
    {
        public List<BackBoneResponse> Items { get; set; } = new();
    }
    public class ValidateBackBoneNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Backbones.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Backbones.ClassName;
    }
    public class DeleteGroupBackBoneRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of BackBone";

        public override string ClassName => StaticClass.Backbones.ClassName;

        public HashSet<BackBoneResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Backbones.EndPoint.DeleteGroup;
    }
}
