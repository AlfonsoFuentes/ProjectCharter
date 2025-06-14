using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Valves.Records
{
    public class GetValveByIdRequest : GetByIdMessageResponse, IGetById
    {
      
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Valves.EndPoint.GetById;
        public override string ClassName => StaticClass.Valves.ClassName;
    }

}
