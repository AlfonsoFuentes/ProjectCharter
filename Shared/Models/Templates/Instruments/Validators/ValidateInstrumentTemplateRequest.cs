using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.NozzleTemplates;

namespace Shared.Models.Templates.Instruments.Validators
{
    public class ValidateInstrumentTemplateRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EndPointName => StaticClass.InstrumentTemplates.EndPoint.Validate;
        public override string Legend => Name;
        public override string ClassName => StaticClass.InstrumentTemplates.ClassName;    
        public int SignalType { get; set; } 
        public string Model { get; set; } = string.Empty;
        public int Material { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int VariableInstrument { get; set; }
        public int ModifierVariable { get; set; }      
        public string Brand { get; set; } = string.Empty; 
        public List<NozzleTemplateResponse> NozzleTemplates { get; set; } = new();

    }

}
