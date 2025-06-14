using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Resources.Mappers
{
    public class ChangeResourceOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Resources.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Resources.ClassName;
    }

}
