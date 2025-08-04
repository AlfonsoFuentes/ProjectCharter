using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BIGWIPTanks;
using Shared.Models.FinishingLines.SKUs;
using Shared.Models.FinishingLines.WIPTankLines;

namespace Shared.Models.FinishingLines.ProductionScheduleItems
{



    public class ProductionScheduleItemResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.ProductionScheduleItems.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid ProductionLineAssigmentId { get; set; }
        public Guid ProductionLineId { get; set; }
        public SKUResponse? Sku { get; set; } = null!;
        public string SkuName => Sku!.Name ?? string.Empty;

        public Mass MassPlanned => new(MassPlannedValue, MassPlannedUnit);
        public double MassPlannedValue { get; set; }
        public string MassPlannedUnit { get; set; } = MassUnits.KiloGram.Name;

       
        public List<BIGWIPTankResponse> InitialLevelBigWips { get; set; } = new();

    }


    public class DeleteProductionScheduleItemRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionScheduleItems.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.Delete;
    }

    public class GetProductionScheduleItemByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.GetById;

        public override string ClassName => StaticClass.ProductionScheduleItems.ClassName;
    }

    public class ProductionScheduleItemGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.GetAll;
    }

    public class ProductionScheduleItemResponseList : IResponseAll
    {
        public List<ProductionScheduleItemResponse> Items { get; set; } = new();
    }

    public class ValidateProductionScheduleItemNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionScheduleItems.ClassName;
    }
    public class DeleteGroupProductionScheduleItemRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of ProductionScheduleItem";

        public override string ClassName => StaticClass.ProductionScheduleItems.ClassName;

        public HashSet<ProductionScheduleItemResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ProductionScheduleItems.EndPoint.DeleteGroup;
    }

}
