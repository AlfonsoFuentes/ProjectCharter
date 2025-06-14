using Shared.Models.EngineeringFluidCodes.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Requests
{
    public class DeleteGroupEngineeringFluidRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of EngineeringFluid";

        public override string ClassName => StaticClass.EngineeringFluidCodes.ClassName;

        public HashSet<EngineeringFluidCodeResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.DeleteGroup;
    }
}
