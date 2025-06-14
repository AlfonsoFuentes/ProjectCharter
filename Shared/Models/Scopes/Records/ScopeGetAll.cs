using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Scopes.Records
{
    public class ScopeGetAll : IGetAll
    {
       
        public string EndPointName => StaticClass.Scopes.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
