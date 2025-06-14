using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Valves.Validators
{
    public class ValidateValveTagRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
       
      
        public string Tag { get; set; }= string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Valves.EndPoint.ValidateTag;

        public override string Legend => Tag;

        public override string ClassName => StaticClass.Valves.ClassName;
    }

}
