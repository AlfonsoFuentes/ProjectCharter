using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.StakeHolders.Requests
{
    public class DeleteStakeHolderRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;
        public override string ClassName => StaticClass.StakeHolders.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.StakeHolders.EndPoint.Delete;
    }
}
