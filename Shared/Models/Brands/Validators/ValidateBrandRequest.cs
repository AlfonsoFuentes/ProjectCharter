using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Brands.Validators
{
   
    public class ValidateBrandRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid DeliverableId { get; set; }

        public string EndPointName => StaticClass.Brands.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Brands.ClassName;
    }
}
