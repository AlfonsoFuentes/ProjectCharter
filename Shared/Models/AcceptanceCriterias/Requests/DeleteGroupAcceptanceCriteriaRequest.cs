using Shared.Models.AcceptanceCriterias.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.AcceptanceCriterias.Requests
{
    public class DeleteGroupAcceptanceCriteriaRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of AcceptanceCriteria";

        public override string ClassName => StaticClass.AcceptanceCriterias.ClassName;

        public HashSet<AcceptanceCriteriaResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.DeleteGroup;
        public Guid ProjectId {  get; set; }    
    }
}
