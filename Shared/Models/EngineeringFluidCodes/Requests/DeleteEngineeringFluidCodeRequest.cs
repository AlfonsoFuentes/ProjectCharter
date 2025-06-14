using Shared.Models.ExpertJudgements.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Requests
{
    public class DeleteEngineeringFluidCodeRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClass.EngineeringFluidCodes.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.Delete;
    }
}
