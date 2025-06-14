using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Bennefits.Requests
{
    public class DeleteBennefitRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
  
        public override string ClassName => StaticClass.Bennefits.ClassName;

        public Guid Id { get; set; }
        public Guid ProjectId {  get; set; }

        public string EndPointName => StaticClass.Bennefits.EndPoint.Delete;
    }
}
