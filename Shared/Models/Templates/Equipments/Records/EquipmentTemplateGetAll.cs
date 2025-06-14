using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Templates.Equipments.Records
{
    public class EquipmentTemplateGetAll : IGetAll
    {
        public string EndPointName => StaticClass.EquipmentTemplates.EndPoint.GetAll;
    }
}
