using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Requirements.Requests
{
    public class DeleteRequirementRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Requirements.ClassName;
       
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Requirements.EndPoint.Delete;
    }
}
