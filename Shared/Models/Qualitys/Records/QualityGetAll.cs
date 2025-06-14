using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Qualitys.Records
{
    public class QualityGetAll : IGetAll
    {
    
        public string EndPointName => StaticClass.Qualitys.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
