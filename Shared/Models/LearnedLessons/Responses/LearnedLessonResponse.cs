using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.LearnedLessons.Responses
{
    public class LearnedLessonResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.LearnedLessons.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.LearnedLessons.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);




        public Guid ProjectId { get; set; }
    }
}
