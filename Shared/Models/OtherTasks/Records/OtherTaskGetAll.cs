using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.OtherTasks.Records
{
    public class OtherTaskGetAll : IGetAll
    {
    
        public string EndPointName => StaticClass.OtherTasks.EndPoint.GetAll;
    
    }
}
