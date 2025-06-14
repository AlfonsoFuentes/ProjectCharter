using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Meetings.Records
{
   public class GetMeetingByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Meetings.EndPoint.GetById;
        public override string ClassName => StaticClass.Meetings.ClassName;
    }

}
