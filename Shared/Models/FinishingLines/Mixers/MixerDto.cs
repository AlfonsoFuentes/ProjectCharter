using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.MixerBackbones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.FinishingLines.Mixers
{



    public class MixerResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Mixers.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.Mixers.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public List<MixerBackboneResponse> Capabilities { get; set; } = new List<MixerBackboneResponse>();

     
        public Time CleaningTime => new Time(CleaningTimeValue, TimeUnits.Hour);

       
        public double CleaningTimeValue { get; set; } = 8;
        public string CleaningTimeUnit { get; set; } = TimeUnits.Hour.Name;
       
    }


    public class DeleteMixerRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Mixers.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Mixers.EndPoint.Delete;
    }

    public class GetMixerByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Mixers.EndPoint.GetById;

        public override string ClassName => StaticClass.Mixers.ClassName;
    }

    public class MixerGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Mixers.EndPoint.GetAll;
    }

    public class MixerResponseList : IResponseAll
    {
        public List<MixerResponse> Items { get; set; } = new();
    }

    public class ValidateMixerNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Mixers.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Mixers.ClassName;
    }
    public class DeleteGroupMixerRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Mixer";

        public override string ClassName => StaticClass.Mixers.ClassName;

        public HashSet<MixerResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.Mixers.EndPoint.DeleteGroup;
    }

}
