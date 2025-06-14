using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Pipings.Validators
{
    public class ValidatePipeTemplateRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;


        public string EndPointName => StaticClass.PipeTemplates.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.PipeTemplates.ClassName;

        public string Material { get; set; } = string.Empty;
        public string Diameter { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public bool Insulation { get; set; }
       

    }

}
