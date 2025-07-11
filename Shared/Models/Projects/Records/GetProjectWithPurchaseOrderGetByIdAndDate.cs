using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Projects.Records
{
    public class GetProjectWithPurchaseOrderGetByIdAndDate : IGetById
    {

        public string EndPointName => StaticClass.Projects.EndPoint.GetByIdWithPurchaseOrderAndDate;
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
     

    }
}
