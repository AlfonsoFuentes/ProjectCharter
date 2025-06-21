using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.OtherTasks.Records
{
    public class OtherTaskGetAllByProject : IGetAll
    {

        public string EndPointName => StaticClass.OtherTasks.EndPoint.GetAllByProject;
        public Guid ProjectId { get; set; }
    }
}
