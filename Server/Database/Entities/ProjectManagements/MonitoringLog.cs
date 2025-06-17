namespace Server.Database.Entities.ProjectManagements
{
    public class MonitoringLog : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? InitialDate { get; set; } 
        public DateTime? EndDate { get; set; }
        public string? ClosingText { get; set; } = string.Empty;
        public static MonitoringLog Create(Guid ProjectId, int Order)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ProjectId = ProjectId,
                Order = Order,


            };
        }





    }
}
