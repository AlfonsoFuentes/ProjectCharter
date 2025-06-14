using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Validators
{
    public class ValidateBasicValveTagRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
       
      
        public string Tag { get; set; }= string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.BasicValves.EndPoint.ValidateTag;

        public override string Legend => Tag;

        public override string ClassName => StaticClass.BasicValves.ClassName;
    }

}
