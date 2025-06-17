using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Warnings.Records
{
    public class WarningGetByProjectId : IGetAll
    {

        public string EndPointName => StaticClass.Warnings.EndPoint.GetById;
        public Guid ProjectId { get; set; }

    }
}
