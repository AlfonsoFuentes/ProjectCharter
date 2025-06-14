using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Assumptions.Requests
{
    public class DeleteAssumptionRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
      
        public override string ClassName => StaticClass.Assumptions.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Assumptions.EndPoint.Delete;
    }
}
