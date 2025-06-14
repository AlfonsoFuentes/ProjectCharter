using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.KnownRisks.Records
{
    public class KnownRiskGetAll : IGetAll
    {
        
        public string EndPointName => StaticClass.KnownRisks.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
