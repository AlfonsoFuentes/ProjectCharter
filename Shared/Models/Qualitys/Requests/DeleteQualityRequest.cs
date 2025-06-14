using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Qualitys.Requests
{
    public class DeleteQualityRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.Qualitys.ClassName;
      
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Qualitys.EndPoint.Delete;
    }
}
