using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Records
{
    public class GetEngineeringDesignByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.EngineeringDesigns.EndPoint.GetById;
        public override string ClassName => StaticClass.EngineeringDesigns.ClassName;
    }

}
