namespace Shared.Models.BudgetItems.IndividualItems.Taxs.Responses
{
    public class TaxItemResponse : BaseResponse
    {
        public Guid? BudgetItemId { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;

        public double Budget { set; get; }
        public bool Selected { get; set; }
    }
}
