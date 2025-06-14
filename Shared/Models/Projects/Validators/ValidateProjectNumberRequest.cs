using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Projects.Validators
{
    public class ValidateProjectNumberRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string ProjectNumber { get; set; } = string.Empty;


        public string EndPointName => StaticClass.Projects.EndPoint.ValidateProjectNumber;

        public override string Legend => ProjectNumber;

        public override string ClassName => StaticClass.Projects.ClassName;
    }
}
