using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.InitialLevelBigWips;
using Shared.Models.FinishingLines.ProductionLineAssignments;

namespace Shared.Models.FinishingLines.ProductionPlans
{



    public class ProductionPlanResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ProductionPlans.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.ProductionPlans.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public List<InitialLevelBigWipResponse> InitialLevelBigWips { get; set; } = new List<InitialLevelBigWipResponse>();

        public List<ProductionLineAssignmentResponse> LineAssignments { get; set; } = new List<ProductionLineAssignmentResponse>();
    }


    public class DeleteProductionPlanRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionPlans.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionPlans.EndPoint.Delete;
    }

    public class GetProductionPlanByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionPlans.EndPoint.GetById;

        public override string ClassName => StaticClass.ProductionPlans.ClassName;
    }

    public class ProductionPlanGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ProductionPlans.EndPoint.GetAll;
    }

    public class ProductionPlanResponseList : IResponseAll
    {
        public List<ProductionPlanResponse> Items { get; set; } = new();
    }

    public class ValidateProductionPlanNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.ProductionPlans.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionPlans.ClassName;
    }
    public class DeleteGroupProductionPlanRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of ProductionPlan";

        public override string ClassName => StaticClass.ProductionPlans.ClassName;

        public HashSet<ProductionPlanResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ProductionPlans.EndPoint.DeleteGroup;
    }

}
