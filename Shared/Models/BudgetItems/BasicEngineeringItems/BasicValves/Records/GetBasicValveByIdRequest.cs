using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Records
{
    public class GetBasicValveByIdRequest : GetByIdMessageResponse, IGetById
    {
      
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.BasicValves.EndPoint.GetById;
        public override string ClassName => StaticClass.BasicValves.ClassName;
    }

}
