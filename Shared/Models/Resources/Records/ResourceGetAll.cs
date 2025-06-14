using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Resources.Records
{
    public class ResourceGetAll : IGetAll
    {
      
        public string EndPointName => StaticClass.Resources.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
