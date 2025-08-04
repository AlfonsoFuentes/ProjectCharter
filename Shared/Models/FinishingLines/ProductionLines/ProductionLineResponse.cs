using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.LineSpeeds;
using Shared.Models.FinishingLines.WIPTankLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.FinishingLines.ProductionLines
{



    public class ProductionLineResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ProductionLines.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.ProductionLines.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        [JsonIgnore]
        public Time FormatChangeTime => new Time(FormatChangeTimeValue, TimeUnits.Minute);
        public double FormatChangeTimeValue { get; set; }
        public string FormatChangeTimeUnit { get; set; } = TimeUnits.Minute.Name;
        [JsonIgnore]
        public Time CleaningTime => new Time(CleaningTimeValue, TimeUnits.Minute);
        public double CleaningTimeValue { get; set; }
        public string CleaningTimeUnit { get; set; } = TimeUnits.Minute.Name;

        public List<LineSpeedResponse> LineSpeeds { get; set; } = new List<LineSpeedResponse>();
 
        public List<WIPTankLineResponse> WIPTanks { get; set; } = new List<WIPTankLineResponse>();
    }


    public class DeleteProductionLineRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionLines.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionLines.EndPoint.Delete;
    }

    public class GetProductionLineByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductionLines.EndPoint.GetById;

        public override string ClassName => StaticClass.ProductionLines.ClassName;
    }

    public class ProductionLineGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ProductionLines.EndPoint.GetAll;
    }

    public class ProductionLineResponseList : IResponseAll
    {
        public List<ProductionLineResponse> Items { get; set; } = new();
    }

    public class ValidateProductionLineNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.ProductionLines.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductionLines.ClassName;
    }
    public class DeleteGroupProductionLineRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of ProductionLine";

        public override string ClassName => StaticClass.ProductionLines.ClassName;

        public HashSet<ProductionLineResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.ProductionLines.EndPoint.DeleteGroup;
    }

}
