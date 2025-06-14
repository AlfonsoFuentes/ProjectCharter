using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Meetings.Records
{
    public class MeetingGetAll : IGetAll
    {
        
        public string EndPointName => StaticClass.Meetings.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
