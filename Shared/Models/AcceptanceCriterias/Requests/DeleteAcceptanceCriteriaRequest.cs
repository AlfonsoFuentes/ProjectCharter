using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.AcceptanceCriterias.Requests
{
    public class DeleteAcceptanceCriteriaRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.AcceptanceCriterias.ClassName;
    
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.Delete;
    }
}
