using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.LearnedLessons.Records
{
   public class GetLearnedLessonByIdRequest : GetByIdMessageResponse, IGetById
    {
   
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.LearnedLessons.EndPoint.GetById;
        public override string ClassName => StaticClass.LearnedLessons.ClassName;
    }

}
