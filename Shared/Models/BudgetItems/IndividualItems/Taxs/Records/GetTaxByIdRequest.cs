using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Taxs.Records
{
    public class GetTaxByIdRequest : GetByIdMessageResponse, IGetById
    {
    
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Taxs.EndPoint.GetById;
        public override string ClassName => StaticClass.Taxs.ClassName;
    }

}
