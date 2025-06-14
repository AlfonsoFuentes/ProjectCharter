using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Records
{
    public class GetBasicInstrumentByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.BasicInstruments.EndPoint.GetById;
        public override string ClassName => StaticClass.BasicInstruments.ClassName;
    }

}
