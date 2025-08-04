using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using System.Text.Json.Serialization;

namespace Shared.Models.FinishingLines.WIPTankLines
{



    public class WIPTankLineResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.WIPTankLines.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.WIPTankLines.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid LineId { get; set; }
        [JsonIgnore]
        public Mass Capacity => new Mass(CapacityValue, MassUnits.KiloGram);
        public double CapacityValue { get; set; } = 2000;
        public string CapacityUnit { get; set; } = MassUnits.KiloGram.Name;

        public double CleaningTimeValue { get; set; }
        public string CleaningTimeUnit { get; set; } = TimeUnits.Minute.Name;
        [JsonIgnore]
        public Time CleaningTime => new Time(CleaningTimeValue, TimeUnits.Minute);
        public double MinimumLevelPercentage { get; set; }
    }


    public class DeleteWIPTankLineRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.WIPTankLines.ClassName;

        public Guid LineId { get; set; }
        public Guid Id { get; set; }
        public string EndPointName => StaticClass.WIPTankLines.EndPoint.Delete;
    }

    public class GetWIPTankLineByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.WIPTankLines.EndPoint.GetById;

        public override string ClassName => StaticClass.WIPTankLines.ClassName;
    }

    public class WIPTankLineGetAll : IGetAll
    {
        public string EndPointName => StaticClass.WIPTankLines.EndPoint.GetAll;
        public Guid ProductionLineId { get; set; } = Guid.Empty;
    }

    public class WIPTankLineResponseList : IResponseAll
    {
        public List<WIPTankLineResponse> Items { get; set; } = new();
    }

    public class ValidateWIPTankLineNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.WIPTankLines.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.WIPTankLines.ClassName;
    }
    public class DeleteGroupWIPTankLineRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of WIPTankLine";

        public override string ClassName => StaticClass.WIPTankLines.ClassName;

        public HashSet<WIPTankLineResponse> SelecteItems { get; set; } = null!;
        public Guid LineId { get; set; }
        public string EndPointName => StaticClass.WIPTankLines.EndPoint.DeleteGroup;
    }

}
