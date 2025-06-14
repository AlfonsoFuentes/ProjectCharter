using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Taxs.Records
{
    public class GetBudgetItemsToApplyTaxRequest : GetByIdMessageResponse, IGetAll
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Taxs.EndPoint.GetBudgetItemsToApplyTaxById;
        public override string ClassName => StaticClass.Taxs.ClassName;
    }

}
