using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Requests
{
    public class DeleteBasicValveRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public Guid ProjectId { get; set; }
        public override string ClassName => StaticClass.BasicValves.ClassName;
        
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BasicValves.EndPoint.Delete;


    }
}
