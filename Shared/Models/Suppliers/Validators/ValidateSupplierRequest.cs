using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.Suppliers.Validators
{
   
    public class ValidateSupplierNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
       
        public string EndPointName => StaticClass.Suppliers.EndPoint.ValidateName;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Suppliers.ClassName;
    }
    public class ValidateSupplierVendorCodeRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Suppliers.EndPoint.ValidateVendorCode;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Suppliers.ClassName;
    }
    public class ValidateSupplierNickNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;

        public string EndPointName => StaticClass.Suppliers.EndPoint.ValidateNickName;

        public override string Legend => Name;

        public override string ClassName => StaticClass.Suppliers.ClassName;
    }
}
