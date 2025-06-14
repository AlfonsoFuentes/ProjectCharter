using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.Equipments.Responses;

namespace Shared.Models.Templates.Equipments.Requests
{
    public class DeleteGroupEquipmentTemplatesRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Equipment Templates";

        public override string ClassName => StaticClass.EquipmentTemplates.ClassName;

        public HashSet<EquipmentTemplateResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.EquipmentTemplates.EndPoint.DeleteGroup;
    }
}
