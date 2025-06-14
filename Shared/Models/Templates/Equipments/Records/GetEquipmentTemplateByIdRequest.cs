using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Equipments.Records
{

    public class GetEquipmentTemplateByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.EquipmentTemplates.EndPoint.GetById;
        public override string ClassName => StaticClass.EquipmentTemplates.ClassName;
    }
}
