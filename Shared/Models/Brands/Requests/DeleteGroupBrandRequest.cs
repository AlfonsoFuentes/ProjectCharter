using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Brands.Requests
{
    public class DeleteGroupBrandRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Brand";

        public override string ClassName => StaticClass.Brands.ClassName;

        public HashSet<BrandResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Brands.EndPoint.DeleteGroup;
    }
}
