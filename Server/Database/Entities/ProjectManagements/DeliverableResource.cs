namespace Server.Database.Entities.ProjectManagements
{
    public class DeliverableResource : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        //public GanttTask GanttTask { get; set; } = null!;
        //public Guid GanttTaskId { get; set; }
        public Guid ResourceId { get; set; }
        public string Avalabilty { get; set; } = string.Empty;

    }
}
