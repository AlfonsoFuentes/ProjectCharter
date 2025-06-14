using Server.Database.Contracts;
using Shared.Enums.AccesoryCategoryEnums;

namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Pipings
{
    public class PipingCategory : AuditableEntity<Guid>, ITenantCommon
    {
        public string Category { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public ICollection<PipingAccesoryCodeBrand> PipingAccesoryCodeBrands { get; set; } = new List<PipingAccesoryCodeBrand>();
        public ICollection<PipingAccesory> PipingAccesories { get; set; } = new List<PipingAccesory>();

        public PipingAccesoryImage? PipingAccesoryImage { get; set; } = null!;
        public Guid? PipingAccesoryImageId { get; set; }
        public static PipingCategory Create(AccesoryCategoryEnum categoryEnum)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Category = categoryEnum.Name,

            };

        }
        public PipingAccesoryCodeBrand AddBrandCode(string brand, string code)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Brand = brand,
                Code = code,
                PipingCategoryId = Id,
            };
        }
        public PipingAccesory AddPipingAccesory(string name)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                PipingCategoryId = Id,
            };
        }
    }

}
