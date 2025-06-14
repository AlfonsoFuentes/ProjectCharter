using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Bennefits.Mappers
{
    public class ChangeBennefitOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Bennefits.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Bennefits.ClassName;
    }

}
