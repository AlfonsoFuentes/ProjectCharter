using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.StakeHolderInsideProjects.Records
{
    public class StakeHolderInsideProjectGetAll : IGetAll
    {
        public string EndPointName => StaticClass.StakeHolderInsideProjects.EndPoint.GetAll;
        public Guid ProjectId { get; set; }

    }
}
