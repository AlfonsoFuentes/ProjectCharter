using Shared.Models.Meetings.Requests;
using Shared.Models.Meetings.Responses;

namespace Shared.Models.Meetings.Mappers
{
    public static class MeetingMapper
    {
        public static ChangeMeetingOrderDowmRequest ToDown(this MeetingResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
                Order = response.Order,


            };
        }
        public static ChangeMeetingOrderUpRequest ToUp(this MeetingResponse response)
        {
            return new()
            {
                ProjectId = response.ProjectId,
                Id = response.Id,
                Name = response.Name,
                Order = response.Order,
            };
        }
        public static UpdateMeetingRequest ToUpdate(this MeetingResponse response)
        {
            return new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = response.ProjectId,
            };
        }
        public static CreateMeetingRequest ToCreate(this MeetingResponse response)
        {
            return new()
            {

                Name = response.Name,
                ProjectId = response.ProjectId,
          
            };
        }
    }

}
