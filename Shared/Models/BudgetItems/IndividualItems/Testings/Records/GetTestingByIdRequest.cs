using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Testings.Records
{
    public class GetTestingByIdRequest : GetByIdMessageResponse, IGetById
    {
      
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Testings.EndPoint.GetById;
        public override string ClassName => StaticClass.Testings.ClassName;
    }

}
