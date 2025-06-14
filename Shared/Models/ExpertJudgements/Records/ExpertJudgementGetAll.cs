using Shared.Models.FileResults.Generics.Records;

namespace Shared.Models.ExpertJudgements.Records
{
    public class ExpertJudgementGetAll : IGetAll
    {

        public string EndPointName => StaticClass.ExpertJudgements.EndPoint.GetAll;
        public Guid ProjectId { get; set; }
    }
}
