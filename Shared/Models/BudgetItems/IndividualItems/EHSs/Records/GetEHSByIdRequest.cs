using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.EHSs.Records
{
    public class GetEHSByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.EHSs.EndPoint.GetById;
        public override string ClassName => StaticClass.EHSs.ClassName;
    }

}
