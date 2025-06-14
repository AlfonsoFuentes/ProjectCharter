using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Validators
{
  
    public class ValidateQualityRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public string EndPointName => StaticClass.Qualitys.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Qualitys.ClassName;
    }
}
