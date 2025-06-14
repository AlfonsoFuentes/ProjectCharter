using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Suppliers.Responses
{
    public class SupplierResponseList: IResponseAll
    {
        public List<SupplierResponse> Items { get; set; } = new();
    }
}
