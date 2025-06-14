using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Acquisitions.Records
{
    public class AcquisitionGetAll : IGetAll
    {
    
        public string EndPointName => StaticClass.Acquisitions.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
