using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.AcceptanceCriterias.Mappers
{
    public class ChangeAcceptanceCriteriaOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.AcceptanceCriterias.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.AcceptanceCriterias.ClassName;
    }

}
