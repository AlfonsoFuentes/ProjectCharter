using Shared.Enums.Materials;
using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.NozzleTemplates;

namespace Shared.Models.Templates.Valves.Validators
{
    public class ValidateValveTemplateRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;


        public string EndPointName => StaticClass.ValveTemplates.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ValveTemplates.ClassName;

        public string Model { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        public int Type { get; set; }

        public List<NozzleTemplateResponse> NozzleTemplates { get; set; } = new();

        public int Material { get; set; } 
        public int ActuadorType { get; set; } 
        public int PositionerType { get; set; } 
        public bool HasFeedBack { get; set; }
        public int Diameter { get; set; } 
        public int FailType { get; set; } 
        public int SignalType { get; set; } 
       
    }

}
