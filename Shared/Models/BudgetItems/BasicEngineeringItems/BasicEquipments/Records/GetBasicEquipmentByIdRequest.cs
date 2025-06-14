using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Records
{
    public class GetBasicEquipmentByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.BasicEquipments.EndPoint.GetById;
        public override string ClassName => StaticClass.BasicEquipments.ClassName;
    }

}
