using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Backgrounds.Requests
{
    public class DeleteBackGroundRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.BackGrounds.ClassName;
 
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BackGrounds.EndPoint.Delete;
    }
}
