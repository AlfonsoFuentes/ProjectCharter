using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.EngineeringFluidCodes.Records
{

    public class GetEngineeringFluidCodeByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.GetById;
        public override string ClassName => StaticClass.EngineeringFluidCodes.ClassName;
    }
}
