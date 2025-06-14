using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Requests
{
    public class DeleteGroupBasicPipeRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Specific equipments";

        public override string ClassName => StaticClass.BasicPipes.ClassName;

        public HashSet<BasicPipeResponse> SelecteItems { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.BasicPipes.EndPoint.DeleteGroup;
    }
}
