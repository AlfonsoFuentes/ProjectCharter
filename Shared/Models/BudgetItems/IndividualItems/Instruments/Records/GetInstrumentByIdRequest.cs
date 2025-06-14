using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Instruments.Records
{
    public class GetInstrumentByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Instruments.EndPoint.GetById;
        public override string ClassName => StaticClass.Instruments.ClassName;
    }

}
