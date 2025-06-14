using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Requirements.Records
{
    public class RequirementGetAll : IGetAll
    {
     
        public string EndPointName => StaticClass.Requirements.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
