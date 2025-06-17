using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Warnings.Records
{
    public class WarningGetAll : IGetAll
    {

        public string EndPointName => StaticClass.Warnings.EndPoint.GetAll;
       

    }
}
