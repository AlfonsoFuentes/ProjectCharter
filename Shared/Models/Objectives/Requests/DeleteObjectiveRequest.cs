using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Objectives.Requests
{
    public class DeleteObjectiveRequest : DeleteMessageResponse, IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.Objectives.ClassName;



        public string EndPointName => StaticClass.Objectives.EndPoint.Delete;
    }
}
