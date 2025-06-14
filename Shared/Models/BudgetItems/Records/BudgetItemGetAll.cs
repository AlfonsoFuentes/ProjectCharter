using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.BudgetItems.Records
{
    public class BudgetItemGetAll : IGetAll
    {
      
        public string EndPointName => StaticClass.BudgetItems.EndPoint.GetAll;
        public Guid ProjectId { get; set; }

    }

}
