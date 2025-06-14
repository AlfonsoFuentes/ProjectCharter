using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Acquisitions.Requests
{
    public class DeleteAcquisitionRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Acquisitions.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Acquisitions.EndPoint.Delete;
    }
}
