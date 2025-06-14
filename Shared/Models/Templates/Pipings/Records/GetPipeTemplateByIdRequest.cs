using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Templates.Pipings.Records
{

    public class GetPipeTemplateByIdRequest : GetByIdMessageResponse, IGetById
    {

        public Guid Id { get; set; }
        public string EndPointName => StaticClass.PipeTemplates.EndPoint.GetById;
        public override string ClassName => StaticClass.PipeTemplates.ClassName;
    }
}
