using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.EngineeringFluidCodes.Responses
{
    public class EngineeringFluidCodeResponseList : IResponseAll
    {
        public List<EngineeringFluidCodeResponse> Items { get; set; } = new();
    }
}
