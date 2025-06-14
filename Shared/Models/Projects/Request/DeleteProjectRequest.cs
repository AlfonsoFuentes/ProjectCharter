using Shared.Models.FileResults.Generics.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Projects.Request
{
    public class DeleteProjectRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string Legend => Name;

        public override string ClassName => StaticClasses.StaticClass.Projects.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClasses.StaticClass.Projects.EndPoint.Delete;
    }
}
