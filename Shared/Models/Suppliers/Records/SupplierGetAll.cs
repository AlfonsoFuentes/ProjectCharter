using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Suppliers.Records
{
    public class SupplierGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Suppliers.EndPoint.GetAll;
    }
}
