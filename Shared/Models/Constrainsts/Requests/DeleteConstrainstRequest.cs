using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Constrainsts.Requests
{
    public class DeleteConstrainstRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Constrainsts.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Constrainsts.EndPoint.Delete;
    }
}
