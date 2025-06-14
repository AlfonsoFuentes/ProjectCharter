using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Mappers
{
    public class ChangeQualityOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.Qualitys.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.Qualitys.ClassName;
    }

}
