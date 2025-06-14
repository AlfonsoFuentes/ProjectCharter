using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.MeetingAgreements.Records
{
   public class GetMeetingAgreementByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.MeetingAgreements.EndPoint.GetById;
        public override string ClassName => StaticClass.MeetingAgreements.ClassName;
    }

}
