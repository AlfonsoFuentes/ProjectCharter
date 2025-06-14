using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Brands.Records
{
    public class BrandGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Brands.EndPoint.GetAll;
    }
}
