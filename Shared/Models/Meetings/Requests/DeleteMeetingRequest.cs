using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Requests
{
    public class DeleteMeetingRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Meetings.ClassName;
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Meetings.EndPoint.Delete;
    }
}
