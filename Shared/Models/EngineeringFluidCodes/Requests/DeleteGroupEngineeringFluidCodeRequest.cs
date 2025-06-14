using Shared.Models.EngineeringFluidCodes.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Requests
{
    public class DeleteGroupEngineeringFluidCodeRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.EngineeringFluidCodes.ClassName;

        public HashSet<EngineeringFluidCodeResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.DeleteGroup;
    }
}
