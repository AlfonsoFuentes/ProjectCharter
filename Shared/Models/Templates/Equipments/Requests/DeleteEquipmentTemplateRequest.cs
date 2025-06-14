using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Equipments.Requests
{
    public class DeleteEquipmentTemplateRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.EquipmentTemplates.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.EquipmentTemplates.EndPoint.Delete;
    }
}
