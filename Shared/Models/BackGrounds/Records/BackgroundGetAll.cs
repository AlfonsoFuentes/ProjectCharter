using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Backgrounds.Records
{
    public class BackGroundGetAll : IGetAll
    {
        public string EndPointName => StaticClass.BackGrounds.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
