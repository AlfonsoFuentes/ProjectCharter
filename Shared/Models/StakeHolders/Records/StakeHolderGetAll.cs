using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.StakeHolders.Records
{
    public class StakeHolderGetAll : IGetAll
    {
        public string EndPointName => StaticClass.StakeHolders.EndPoint.GetAll;
    }
}
