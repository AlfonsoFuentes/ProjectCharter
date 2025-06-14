using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Apps.Records
{
    public class GetCurrentProjectId : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.Apps.EndPoint.GetById;
        public override string ClassName => StaticClass.Apps.ClassName;
    }
}
