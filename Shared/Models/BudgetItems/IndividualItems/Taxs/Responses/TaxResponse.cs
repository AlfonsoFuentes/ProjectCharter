using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Taxs.Responses
{
    public class TaxResponse : BudgetItemWithPurchaseOrdersResponse, IMessageResponse, IRequest
    {


        public string EndPointName => StaticClass.Taxs.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Taxs.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid? GanttTaskId { get; set; }

        public double Percentage { get; set; }
      
        public List<TaxItemResponse> TaxItems { get; set; } = new List<TaxItemResponse>();
        public double BudgetTaxItem => TaxItems.Count == 0 ? 0 : TaxItems.Sum(x => x.Budget);

        double BudgetCalculated => BudgetTaxItem * Percentage / 100;

       
    }
}
