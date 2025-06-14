using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.EngineeringFluidCodes.Records
{
    public class EngineeringFluidCodeGetAll() : IGetAll
    {
        public string EndPointName => StaticClass.EngineeringFluidCodes.EndPoint.GetAll;
    }
}
