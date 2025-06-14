using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.Communications.Records
{
    public class CommunicationGetAll : IGetAll
    {
       
        public string EndPointName => StaticClass.Communications.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
