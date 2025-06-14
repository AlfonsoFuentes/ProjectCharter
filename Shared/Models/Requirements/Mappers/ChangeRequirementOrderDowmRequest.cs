using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Requirements.Mappers
{
    public class ChangeRequirementOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Requirements.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Requirements.ClassName;
    }

}
