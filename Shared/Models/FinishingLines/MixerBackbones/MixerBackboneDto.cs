using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BackBones;
using System.Text.Json.Serialization;

namespace Shared.Models.FinishingLines.MixerBackbones
{



    public class MixerBackboneResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.MixerBackbones.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.MixerBackbones.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid MixerId { get; set; }

        public string BackboneName => Backbone?.Name ?? string.Empty;   

        public BackBoneResponse? Backbone { get; set; } = null!;
        [JsonIgnore]
        public Time BatchCycleTime => new Time(BatchCycleTimeValue, TimeUnits.Minute);

        public double BatchCycleTimeValue { get; set; } = 1;
        public string BatchCycleTimeUnit { get; set; } = TimeUnits.Minute.Name;
        [JsonIgnore]
        public Mass Capacity => new Mass(CapacityValue, MassUnits.KiloGram);
        public double CapacityValue { get; set; } = 1;
        public string CapacityUnit { get; set; } = MassUnits.KiloGram.Name;
        
    }


    public class DeleteMixerBackboneRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.MixerBackbones.ClassName;

        public Guid MixerId { get; set; }
        public Guid BackboneId { get; set; }
        public string EndPointName => StaticClass.MixerBackbones.EndPoint.Delete;
    }

    public class GetMixerBackboneByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.MixerBackbones.EndPoint.GetById;

        public override string ClassName => StaticClass.MixerBackbones.ClassName;
    }

    public class MixerBackboneGetAll : IGetAll
    {
        public string EndPointName => StaticClass.MixerBackbones.EndPoint.GetAll;
    }

    public class MixerBackboneResponseList : IResponseAll
    {
        public List<MixerBackboneResponse> Items { get; set; } = new();
    }

    public class ValidateMixerBackboneNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.MixerBackbones.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.MixerBackbones.ClassName;
    }
    public class DeleteGroupMixerBackboneRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of MixerBackbone";

        public override string ClassName => StaticClass.MixerBackbones.ClassName;

        public HashSet<MixerBackboneResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.MixerBackbones.EndPoint.DeleteGroup;
        public Guid MixerId { get; set; } = Guid.Empty;
    }

}
