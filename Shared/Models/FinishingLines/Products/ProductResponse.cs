using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.ProductComponents;

namespace Shared.Models.FinishingLines.Products
{
    public class ProductResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.Products.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.Products.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public List<ProductComponentResponse> Components { get; set; } = new();
        public double SumPorcentages => Math.Round(Components.Sum(x => x.Percentage));
    }


    public class DeleteProductRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Products.ClassName;
        public Guid Id { get; set; }    
        public string EndPointName => StaticClass.Products.EndPoint.Delete;
    }

    public class GetProductByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.Products.EndPoint.GetById;

        public override string ClassName => StaticClass.Products.ClassName;
    }

    public class ProductGetAll : IGetAll
    {
        public string EndPointName => StaticClass.Products.EndPoint.GetAll;
    }

    public class ProductResponseList : IResponseAll
    {
        public List<ProductResponse> Items { get; set; } = new();
    }

    public class ValidateProductNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Products.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Products.ClassName;
    }
    public class DeleteGroupProductRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of Product";

        public override string ClassName => StaticClass.Products.ClassName;

        public HashSet<ProductResponse> SelecteItems { get; set; } = null!;
   
        public string EndPointName => StaticClass.Products.EndPoint.DeleteGroup;
    }
}
