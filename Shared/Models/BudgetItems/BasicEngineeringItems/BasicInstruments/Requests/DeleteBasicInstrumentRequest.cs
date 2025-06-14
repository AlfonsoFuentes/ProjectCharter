using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Requests
{
    public class DeleteBasicInstrumentRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.BasicInstruments.ClassName;
  
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BasicInstruments.EndPoint.Delete;


    }
}
