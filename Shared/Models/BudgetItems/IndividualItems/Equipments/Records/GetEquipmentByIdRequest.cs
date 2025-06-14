using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Equipments.Records
{
    public class GetEquipmentByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Equipments.EndPoint.GetById;
        public override string ClassName => StaticClass.Equipments.ClassName;
    }

}
