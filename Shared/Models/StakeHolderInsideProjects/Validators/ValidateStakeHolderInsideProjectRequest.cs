using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolderInsideProjects.Validators
{
    public class ValidateStakeHolderInsideProjectRequest : ValidateMessageResponse, IRequest
    {
        public Guid? OriginalStakeHolderId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid StakeHolderId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.StakeHolderInsideProjects.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.StakeHolderInsideProjects.ClassName;
    }
}
