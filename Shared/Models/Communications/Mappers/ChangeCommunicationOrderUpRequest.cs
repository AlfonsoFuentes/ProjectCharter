using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Communications.Mappers
{
    public class ChangeCommunicationOrderUpRequest : UpdateMessageResponse, IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public string EndPointName => StaticClass.Communications.EndPoint.UpdateUp;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Communications.ClassName;
    }
}
