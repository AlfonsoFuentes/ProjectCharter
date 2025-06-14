using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams
{
    public class Brand : AuditableEntity<Guid>, ITenantCommon
    {

        public string Name { get; set; } = string.Empty;
        public static Brand Create()
        {
            return new Brand() { Id = Guid.NewGuid() };
        }



        [ForeignKey("BrandTemplateId")]
        public ICollection<Template> BrandTemplates { get; set; } = new HashSet<Template>();

    }

}
