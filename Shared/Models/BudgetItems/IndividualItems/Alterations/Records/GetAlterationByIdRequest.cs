using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Alterations.Records
{
    public class GetAlterationByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Alterations.EndPoint.GetById;
        public override string ClassName => StaticClass.Alterations.ClassName;
    }

}
