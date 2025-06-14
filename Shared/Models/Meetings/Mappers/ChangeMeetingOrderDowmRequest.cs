using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Mappers
{
    public class ChangeMeetingOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Meetings.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Meetings.ClassName;
    }

}
