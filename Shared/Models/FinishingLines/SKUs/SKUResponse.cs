using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.Products;
using System.Text.Json.Serialization;

namespace Shared.Models.FinishingLines.SKUs
{



    public class SKUResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.SKUs.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.SKUs.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);


        [JsonIgnore]
        public Volume VolumePerEA => new Volume(VolumePerEAValue, VolumeUnits.MilliLiter);
        [JsonIgnore]
        public Mass MassPerEA => new Mass(MassPerEAValue, MassUnits.Gram);
       
        public double VolumePerEAValue { get; set; } = 0;
        public string VolumePerEAUnit { get; set; } = VolumeUnits.MilliLiter.Name;

        public double MassPerEAValue { get; set; } = 0;
        public string MassPerEAUnit { get; set; } = MassUnits.Gram.Name;

      

        public ProductResponse? Product { get; set; } = null!;

        string name => $"{Product?.Name} - {VolumePerEA.ToString()} ";
        public override string Name
        {
            get => name;
            set => base.Name = value ?? string.Empty;
        }



    }


    public class DeleteSKURequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.SKUs.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.SKUs.EndPoint.Delete;
    }

    public class GetSKUByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.SKUs.EndPoint.GetById;

        public override string ClassName => StaticClass.SKUs.ClassName;
    }

    public class SKUGetAll : IGetAll
    {
        public string EndPointName => StaticClass.SKUs.EndPoint.GetAll;

        public Guid ProductionLineId { get; set; } = Guid.Empty;
    }

    public class SKUResponseList : IResponseAll
    {
        public List<SKUResponse> Items { get; set; } = new();
    }

    public class ValidateSKUNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.SKUs.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.SKUs.ClassName;
    }
    public class DeleteGroupSKURequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of SKU";

        public override string ClassName => StaticClass.SKUs.ClassName;

        public HashSet<SKUResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.SKUs.EndPoint.DeleteGroup;
    }

}
