using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Templates.Equipments.Responses
{
    public class EquipmentTemplateResponseList : IResponseAll
    {
        public List<EquipmentTemplateResponse> Items { get; set; } = new();
    }
}
