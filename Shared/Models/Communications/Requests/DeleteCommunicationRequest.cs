using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Communications.Requests
{
    public class DeleteCommunicationRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Communications.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Communications.EndPoint.Delete;
    }
}
