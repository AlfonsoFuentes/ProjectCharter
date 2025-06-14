namespace Shared.Models.BudgetItems.BasicEngineeringItems
{ 
    public class BasicResponse : BaseResponse
    {
        public virtual string TagNumber { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public double BudgetUSD { get; set; }
        public virtual string Tag { get; } = string.Empty;
        
    }
}
