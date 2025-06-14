using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Requests
{
    public class DeleteGroupBasicValveRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Specific equipments";

        public override string ClassName => StaticClass.BasicValves.ClassName;

        public HashSet<BasicValveResponse> SelecteItems { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.BasicValves.EndPoint.DeleteGroup;
    }
}
