
using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;
using Shared.StaticClasses;

namespace Shared.Models.Projects.Records
{

    public class GetProjectByIdRequest : GetByIdMessageResponse, IGetById
    {
       
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Projects.EndPoint.GetById;
        public override string ClassName => StaticClass.Projects.ClassName;
    }
}
