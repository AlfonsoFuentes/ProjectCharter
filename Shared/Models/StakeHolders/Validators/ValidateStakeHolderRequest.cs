using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolders.Validators
{
    
    public class ValidateStakeHolderRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
      
        public string EndPointName => StaticClass.StakeHolders.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.StakeHolders.ClassName;
    }
}
