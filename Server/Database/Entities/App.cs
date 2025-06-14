namespace Server.Database.Entities
{
    public class App : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public Guid CurrentProjectId { get; set; }
        public static App Create()
        {
            return new App()
            {
                Id = Guid.NewGuid(),
            };

        }
    }


}
