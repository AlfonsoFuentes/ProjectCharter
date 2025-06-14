using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Requests
{
    public class DeleteBasicPipeRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.BasicPipes.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BasicPipes.EndPoint.Delete;


    }
}
