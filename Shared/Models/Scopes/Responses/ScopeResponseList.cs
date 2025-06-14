using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.Scopes.Responses
{
    public class ScopeResponseList : IResponseAll
    {
        public List<ScopeResponse> Items { get; set; } = new();
       public Guid ProjectId { get; set; }
    }
}
