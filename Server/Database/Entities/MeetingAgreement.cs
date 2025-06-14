using Server.Database.Contracts;

namespace Server.Database.Entities
{
    public class MeetingAgreement : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Meeting Meeting { get; set; } = null!;
        public Guid MeetingId { get; set; }

        public static MeetingAgreement Create(Guid MeetingId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                MeetingId = MeetingId,
            };
        }

    }
}
