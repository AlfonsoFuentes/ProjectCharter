using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.OtherTasks.Mappers
{
    public class ChangeOtherTaskOrderDowmRequest : UpdateMessageResponse, IRequest
    {

        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string EndPointName => StaticClass.OtherTasks.EndPoint.UpdateDown;
        public int Order { get; set; }
        public override string Legend => Name;

        public override string ClassName => StaticClass.OtherTasks.ClassName;
    }

}
