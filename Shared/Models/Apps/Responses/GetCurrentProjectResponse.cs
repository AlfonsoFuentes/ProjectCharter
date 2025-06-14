using Shared.Models.FileResults.Generics.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Apps.Responses
{
    public class GetCurrentProjectResponse : BaseResponse
    {
        
        public Guid CurrentProjectId { get; set; }
    }
}
