using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Testings.Requests
{
    public class DeleteTestingRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.Testings.ClassName;
        public Guid? GanttTaskId { get; set; }
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Testings.EndPoint.Delete;


    }
}
