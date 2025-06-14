using Server.Database.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class PipingAccesoryImage : AuditableEntity<Guid>, ITenantCommon
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        [ForeignKey("PipingAccesoryImageId")]
        public ICollection<PipingCategory> PipingCategories { get; set; } = new List<PipingCategory>();
        public static PipingAccesoryImage Create(string name, string image)
        {
            return new PipingAccesoryImage()
            {
                Name = name,
                Image = image,
                Id = Guid.NewGuid(),
            };
        }
    }

}
