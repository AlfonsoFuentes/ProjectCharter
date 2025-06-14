using Shared.Enums.Materials;
using Shared.ExtensionsMetods;
using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.NozzleTemplates;

namespace Shared.Models.Templates.Equipments.Responses
{
    public class EquipmentTemplateResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string Reference { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public MaterialEnum InternalMaterial { get; set; } = MaterialEnum.None;
        public MaterialEnum ExternalMaterial { get; set; } = MaterialEnum.None;
        public double Value { get; set; }
        
        public string Type { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string TagLetter { get; set; } = string.Empty;
        public BrandResponse? Brand { get; set; }
        public string BrandName => Brand == null ? string.Empty : Brand.Name;
        public List<NozzleTemplateResponse> Nozzles { get; set; } = new();
        public string EndPointName => StaticClass.EquipmentTemplates.EndPoint.CreateUpdate;

        public string Legend => $"{Type} {Model}";

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.EquipmentTemplates.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
    }
}
