using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Records
{
    public class GetBasicPipeByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.BasicPipes.EndPoint.GetById;
        public override string ClassName => StaticClass.BasicPipes.ClassName;
    }

}
