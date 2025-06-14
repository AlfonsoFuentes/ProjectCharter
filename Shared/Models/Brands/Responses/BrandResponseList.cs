using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Brands.Responses
{
    public class BrandResponseList: IResponseAll
    {
        public List<BrandResponse> Items { get; set; } = new();
    }
}
