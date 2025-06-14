using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Constrainsts.Records
{
    public class ConstrainstGetAll : IGetAll
    {
 
        public string EndPointName => StaticClass.Constrainsts.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
