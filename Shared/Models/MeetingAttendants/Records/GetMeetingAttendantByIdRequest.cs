using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAttendants.Records
{
   public class GetMeetingAttendantByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.MeetingAttendants.EndPoint.GetById;
        public override string ClassName => StaticClass.MeetingAttendants.ClassName;
    }

}
