using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Enums.ValvesEnum;
using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.NozzleTemplates;

namespace Shared.Models.Templates.Valves.Responses
{
    public class ValveTemplateResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string Model { get; set; } = string.Empty;
        public MaterialEnum Material { get; set; } = MaterialEnum.None;
        public ActuatorTypeEnum ActuatorType { get; set; } = ActuatorTypeEnum.None;
        public PositionerTypeEnum PositionerType { get; set; } = PositionerTypeEnum.None;
        public bool HasFeedBack { get; set; }
        public NominalDiameterEnum Diameter { get; set; } = NominalDiameterEnum.None;
        public FailTypeEnum FailType { get; set; } = FailTypeEnum.None;
        public SignalTypeEnum SignalType { get; set; } = SignalTypeEnum.None;
        public ConnectionTypeEnum ConnectionType { get; set; } = ConnectionTypeEnum.None;
        public double Value { get; set; }
   
        public ValveTypesEnum Type { get; set; } = ValveTypesEnum.None;
     
        public string TagLetter { get; set; } = string.Empty;
        public BrandResponse? Brand { get; set; } 
        public string BrandName => Brand == null ? string.Empty : Brand.Name;
        public List<NozzleTemplateResponse> Nozzles { get; set; } = new();

        public string Legend => $"{Type} {Model}";
        public string EndPointName => StaticClass.ValveTemplates.EndPoint.CreateUpdate;
        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.ValveTemplates.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

    }
}
