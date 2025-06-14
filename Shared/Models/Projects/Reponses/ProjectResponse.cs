using Shared.Enums.CostCenter;
using Shared.Enums.Focuses;
using Shared.Enums.ProjectNeedTypes;
using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Projects.Reponses
{
    public class ProjectResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Projects.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.Projects.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public ProjectNeedTypeEnum ProjectNeedType { get; set; } = ProjectNeedTypeEnum.None;
      
        public DateTime? InitialProjectDate { get; set; } = DateTime.Today;
        public string ProjectNumber { get; set; } = string.Empty;
        public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.None;
        public double PercentageEngineering { get; set; }
        public double PercentageContingency { get; set; }

        public double PercentageTaxProductive { get; set; }
        public bool IsProductiveAsset { get; set; } = true;
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;

        public int BudgetItems { get; set; } = 0;

    }

}
