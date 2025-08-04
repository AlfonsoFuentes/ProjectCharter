using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.InitialLevelWips;
using Shared.Models.FinishingLines.ProductionLines;
using Shared.Models.FinishingLines.ProductionScheduleItems;
using Shared.Models.FinishingLines.WIPTankLines;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.FinishingLines.ProductionLineAssignments
{



    public class ProductionLineAssignmentResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.ProductionLineAssignments.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid PlanId { get; set; }
        public ProductionLineResponse ProductionLine { get; set; } = null!;
        public string LineName => ProductionLine!.Name ?? string.Empty;
        public Guid ProductionLinedId => ProductionLine == null ? Guid.Empty : ProductionLine!.Id;
        public List<ProductionScheduleItemResponse> ScheduleItems { get; set; } = new List<ProductionScheduleItemResponse>();
        public List<ProductionScheduleItemResponse> OrderedScheduleItems => ScheduleItems.OrderBy(x => x.Order).ToList();
        public int LastOrder => OrderedScheduleItems.Count > 0 ? OrderedScheduleItems.Max(x => x.Order) : 0;
        public List<InitialLevelWipResponse> InitialLevelWips { get; set; } = new();
    }
    

    public class DeleteProductionLineAssignmentRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionLineAssignments.ClassName;

        public Guid Id { get; set; }
        public Guid PlanId { get; set; }
        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.Delete;
    }

    public class GetProductionLineAssignmentByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.GetById;

        public override string ClassName => StaticClass.ProductionLineAssignments.ClassName;
    }

    public class ProductionLineAssignmentGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.GetAll;
    }

    public class ProductionLineAssignmentResponseList : IResponseAll
    {
        public List<ProductionLineAssignmentResponse> Items { get; set; } = new();
    }

    public class ValidateProductionLineAssignmentNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionLineAssignments.ClassName;
    }
    public class DeleteGroupProductionLineAssignmentRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of ProductionLineAssignment";

        public override string ClassName => StaticClass.ProductionLineAssignments.ClassName;

        public HashSet<ProductionLineAssignmentResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ProductionLineAssignments.EndPoint.DeleteGroup;
    }

}
