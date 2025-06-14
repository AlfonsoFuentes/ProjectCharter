using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.IndividualItems.Paintings.Records
{
    public class GetPaintingByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Paintings.EndPoint.GetById;
        public override string ClassName => StaticClass.Paintings.ClassName;
    }

}
