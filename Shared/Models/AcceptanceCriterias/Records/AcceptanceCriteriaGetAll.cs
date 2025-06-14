using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.AcceptanceCriterias.Records
{
    public class AcceptanceCriteriaGetAll : IGetAll
    {
      
        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
      
    }
}
