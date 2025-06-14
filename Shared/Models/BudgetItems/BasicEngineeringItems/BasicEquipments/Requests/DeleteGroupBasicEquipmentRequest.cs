using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests
{
    public class DeleteGroupBasicEquipmentRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Specific equipments";

        public override string ClassName => StaticClass.BasicEquipments.ClassName;

        public HashSet<BasicEquipmentResponse> SelecteItems { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.BasicEquipments.EndPoint.DeleteGroup;
    }
}
