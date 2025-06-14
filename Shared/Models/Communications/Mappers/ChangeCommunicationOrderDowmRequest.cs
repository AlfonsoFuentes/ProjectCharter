using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Communications.Mappers
{
    public class ChangeCommunicationOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Communications.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Communications.ClassName;
    }

}
