using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Meetings.Responses
{
    public class MeetingResponseList : IResponseAll
    {

        public List<MeetingResponse> Items { get; set; } = new();
    }
}
