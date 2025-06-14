using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Apps.Requests
{
    public class CreateUpdateAppRequest : CreateMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Apps.EndPoint.CreateUpdate;

        public override string Legend => string.Empty;

        public override string ClassName => StaticClass.Apps.ClassName;

    }
}
