using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BackBones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.FinishingLines.ProductComponents
{



    public class ProductComponentResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.ProductComponents.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.ProductComponents.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public double Percentage { get; set; }

        public Guid ProductId { get; set; }
        public BackBoneResponse? Backbone { get; set; } = null!;
        public string BackboneName => Backbone?.Name ?? string.Empty;   
    }


    public class DeleteProductComponentRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductComponents.ClassName;

        public Guid BackBoneId { get; set; }
        public Guid ProductId { get; set; }
        public string EndPointName => StaticClass.ProductComponents.EndPoint.Delete;
    }

    public class GetProductComponentByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.ProductComponents.EndPoint.GetById;

        public override string ClassName => StaticClass.ProductComponents.ClassName;
    }

    public class ProductComponentGetAll : IGetAll
    {
        public string EndPointName => StaticClass.ProductComponents.EndPoint.GetAll;
    }

    public class ProductComponentResponseList : IResponseAll
    {
        public List<ProductComponentResponse> Items { get; set; } = new();
    }

    public class ValidateProductComponentNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.ProductComponents.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.ProductComponents.ClassName;
    }
    public class DeleteGroupProductComponentRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of ProductComponent";

        public override string ClassName => StaticClass.ProductComponents.ClassName;

        public HashSet<ProductComponentResponse> SelecteItems { get; set; } = null!;
        public Guid ProductId { get; set; }
        public string EndPointName => StaticClass.ProductComponents.EndPoint.DeleteGroup;
    }

}
