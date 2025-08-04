using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BackBones;
using System.Text.Json.Serialization;

namespace Shared.Models.FinishingLines.BIGWIPTanks
{



    public class BIGWIPTankResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.BIGWIPTanks.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);


        public BackBoneResponse? Backbone { get; set; } = null!;
        public string BackBoneName => Backbone?.Name ?? string.Empty;
        [JsonIgnore]
        public Time CleaningTime => new Time(CleaningTimeValue, TimeUnits.Minute);
        public double CleaningTimeValue { get; set; }
        public string CleaningTimeUnit { get; set; } = TimeUnits.Minute.Name;
        [JsonIgnore]
        public Mass Capacity => new Mass(CapacityValue, MassUnits.KiloGram);
        public double CapacityValue { get; set; }
        public string CapacityUnit { get; set; } = MassUnits.KiloGram.Name;

        [JsonIgnore]
        public MassFlow InletFlow => new MassFlow(InletFlowValue, MassFlowUnits.Kg_min);
        public double InletFlowValue { get; set; }
        public string InletFlowUnit { get; set; } = MassFlowUnits.Kg_min.Name;

        [JsonIgnore]
        public MassFlow OutletFlow => new MassFlow(OutletFlowValue, MassFlowUnits.Kg_min);
        public double OutletFlowValue { get; set; }
        public string OutletFlowUnit { get; set; } = MassFlowUnits.Kg_min.Name;
        public double MinimumLevelPercentage { get; set; }
    }


    public class DeleteBIGWIPTankRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BIGWIPTanks.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.Delete;
    }

    public class GetBIGWIPTankByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.GetById;

        public override string ClassName => StaticClass.BIGWIPTanks.ClassName;
    }

    public class BIGWIPTankGetAll : IGetAll
    {
        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.GetAll;
    }

    public class BIGWIPTankResponseList : IResponseAll
    {
        public List<BIGWIPTankResponse> Items { get; set; } = new();
    }

    public class ValidateBIGWIPTankNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.BIGWIPTanks.ClassName;
    }
    public class DeleteGroupBIGWIPTankRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of BIGWIPTank";

        public override string ClassName => StaticClass.BIGWIPTanks.ClassName;

        public HashSet<BIGWIPTankResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.BIGWIPTanks.EndPoint.DeleteGroup;
    }

}
