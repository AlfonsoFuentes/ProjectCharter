using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Projects.Records
{
    public class ProjectGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Projects.EndPoint.GetAll;
    }
}
