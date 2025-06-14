using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests
{
    public class DeleteGroupBasicInstrumentRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Specific instruments";

        public override string ClassName => StaticClass.BasicInstruments.ClassName;

        public HashSet<BasicInstrumentResponse> SelecteItems { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.BasicInstruments.EndPoint.DeleteGroup;
    }
}
