using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Warnings.Responses
{
    public class WarningResponseList : IResponseAll
    {
        public List<WarningResponse> Items { get; set; } = new List<WarningResponse>();
    }
}
