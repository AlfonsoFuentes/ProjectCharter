using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Assumptions.Records
{
    public class AssumptionGetAll : IGetAll
    {
  
        public string EndPointName => StaticClass.Assumptions.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
