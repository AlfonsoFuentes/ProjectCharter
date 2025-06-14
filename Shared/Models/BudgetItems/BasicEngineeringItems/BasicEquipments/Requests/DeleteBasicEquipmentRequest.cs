using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests
{
    public class DeleteBasicEquipmentRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.BasicEquipments.ClassName;
  
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BasicEquipments.EndPoint.Delete;


    }
}
