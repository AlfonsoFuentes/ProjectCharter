using Server.Database.Entities.PurchaseOrders;
using Shared.Enums.CurrencyEnums;
using Shared.Models.Suppliers.Records;
using Shared.Models.Suppliers.Responses;

namespace Server.EndPoint.Suppliers.Queries
{
    public static class GetSupplierByIdEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.GetById, async (GetSupplierByIdRequest request, IQueryRepository Repository) =>
                {

                    Expression<Func<Supplier, bool>> Criteria = x => x.Id == request.Id;

                    string CacheKey = StaticClass.Suppliers.Cache.GetById(request.Id);
                    var row = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria);

                    if (row == null)
                    {
                        return Result.Fail(request.NotFound);
                    }

                    var response = row.Map();
                    return Result.Success(response);

                });
            }
        }

        public static SupplierResponse Map(this Supplier row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                VendorCode = row.VendorCode,
                Address = row.Address,
                ContactEmail = row.ContactEmail,
                ContactName = row.ContactName,
                NickName = row.NickName,
                PhoneNumber = row.PhoneNumber,
                SupplierCurrency = CurrencyEnum.GetType(row.SupplierCurrency),
                TaxCodeLD = row.TaxCodeLD,
                TaxCodeLP = row.TaxCodeLP,

            };
        }

    }
}
