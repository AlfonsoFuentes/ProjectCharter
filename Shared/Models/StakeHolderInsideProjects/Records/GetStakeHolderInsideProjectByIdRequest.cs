using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolderInsideProjects.Records
{
    public class GetStakeHolderInsideProjectByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.StakeHolderInsideProjects.EndPoint.GetById;
        public override string ClassName => StaticClass.StakeHolders.ClassName;
        public Guid ProjectId { get; set; }
    }
}
