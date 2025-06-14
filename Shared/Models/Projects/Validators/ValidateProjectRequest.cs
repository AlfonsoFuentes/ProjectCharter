using Shared.Models.FileResults.Generics.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Projects.Validators
{
    public class ValidateProjectRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;


        public string EndPointName => StaticClass.Projects.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Projects.ClassName;
    }
}
