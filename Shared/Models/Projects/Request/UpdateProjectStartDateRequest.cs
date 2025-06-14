using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Projects.Request
{
    public class UpdateProjectStartDateRequest : UpdateMessageResponse, IRequest
    {
    
        public override string Legend => "Updated Start Date";

        public override string ClassName => StaticClasses.StaticClass.Projects.ClassName;

        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }

        public string EndPointName => StaticClasses.StaticClass.Projects.EndPoint.UpdateProjectStartDate;
    }
}
