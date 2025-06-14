using Shared.Models.Suppliers.Validators;
using Shared.Models.Backgrounds.Validators;
using Server.Database.Entities.PurchaseOrders;

namespace Server.EndPoint.Suppliers.Validators
{
    public static class ValidateSuppliersNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.ValidateName, async (ValidateSupplierNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Supplier, bool>> CriteriaId = null!;
                    Func<Supplier, bool> CriteriaExist = x => Data.Id == null ?
                    x.Name.Equals(Data.Name) : x.Id != Data.Id.Value && x.Name.Equals(Data.Name);
                    string CacheKey = StaticClass.Suppliers.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }

        }



    }
    public static class ValidateSuppliersNickNameEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.ValidateNickName, async (ValidateSupplierNickNameRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Supplier, bool>> CriteriaId = null!;
                    Func<Supplier, bool> CriteriaExist = x => Data.Id == null ?
                    x.NickName.Equals(Data.NickName) : x.Id != Data.Id.Value && x.NickName.Equals(Data.NickName);
                    string CacheKey = StaticClass.Suppliers.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }

        }



    }
    public static class ValidateSuppliersVendorCodeEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.ValidateVendorCode, async (ValidateSupplierVendorCodeRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Supplier, bool>> CriteriaId = null!;
                    Func<Supplier, bool> CriteriaExist = x => Data.Id == null ?
                    x.VendorCode.Equals(Data.VendorCode) : x.Id != Data.Id.Value && x.NickName.Equals(Data.VendorCode);
                    string CacheKey = StaticClass.Suppliers.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }

        }



    }

}
