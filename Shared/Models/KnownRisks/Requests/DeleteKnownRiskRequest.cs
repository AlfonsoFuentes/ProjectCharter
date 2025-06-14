using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.KnownRisks.Requests
{
    public class DeleteKnownRiskRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.KnownRisks.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.KnownRisks.EndPoint.Delete;
    }
}
