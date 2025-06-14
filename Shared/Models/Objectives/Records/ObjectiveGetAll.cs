using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Objectives.Records
{
    public class ObjectiveGetAll : IGetAll
    {
       
        public string EndPointName => StaticClass.Objectives.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
  
    }
}
