using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.SKUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.FinishingLines.LineSpeeds
{



    public class LineSpeedResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.LineSpeeds.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.LineSpeeds.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public Guid LineId { get; set; }
        [JsonIgnore]    
        public LineVelocity MaxSpeed => new LineVelocity(MaxSpeedValue, LineVelocityUnits.EA_min);
        public SKUResponse? Sku { get; set; } = null!;
        public string Skuname => Sku?.Name ?? string.Empty; 
        public double MaxSpeedValue { get; set; }
        public string MaxSpeedUnit { get; set; } = LineVelocityUnits.EA_min.Name;
        public double PercentageAU { get; set; } = 75;
    }


    public class DeleteLineSpeedRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.LineSpeeds.ClassName;

        public Guid LineId { get; set; }
        public Guid SkuId { get; set; }
        public string EndPointName => StaticClass.LineSpeeds.EndPoint.Delete;
    }

    public class GetLineSpeedByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.LineSpeeds.EndPoint.GetById;

        public override string ClassName => StaticClass.LineSpeeds.ClassName;
    }

    public class LineSpeedGetAll : IGetAll
    {
        public string EndPointName => StaticClass.LineSpeeds.EndPoint.GetAll;
        public Guid LineId { get; set; } = Guid.Empty;
    }

    public class LineSpeedResponseList : IResponseAll
    {
        public List<LineSpeedResponse> Items { get; set; } = new();
    }

    public class ValidateLineSpeedNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.LineSpeeds.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.LineSpeeds.ClassName;
    }
    public class DeleteGroupLineSpeedRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of LineSpeed";

        public override string ClassName => StaticClass.LineSpeeds.ClassName;

        public HashSet<LineSpeedResponse> SelecteItems { get; set; } = null!;
        public Guid LineId { get; set; }
        public string EndPointName => StaticClass.LineSpeeds.EndPoint.DeleteGroup;
    }

}
