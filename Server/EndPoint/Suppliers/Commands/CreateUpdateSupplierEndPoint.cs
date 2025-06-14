using Server.Database.Entities.PurchaseOrders;
using Shared.Models.Suppliers.Requests;
using Shared.Models.Suppliers.Responses;

namespace Server.EndPoint.Suppliers.Commands
{

    public static class CreateUpdateSupplierEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Suppliers.EndPoint.CreateUpdate, async (SupplierResponse Data, IRepository Repository) =>
                {
                    Supplier? row = null!;
                    if (Data.Id == Guid.Empty)
                    {
                        row = Supplier.Create();

                        await Repository.AddAsync(row);
                    }
                    else
                    {
                        row = await Repository.GetByIdAsync<Supplier>(Data.Id);
                        if (row == null) { return Result.Fail(Data.NotFound); }
                        await Repository.UpdateAsync(row);
                    }


                    Data.Map(row);
                    List<string> cache = [.. StaticClass.Suppliers.Cache.Key(row.Id)];

                    var result = await Repository.Context.SaveChangesAndRemoveCacheAsync(cache.ToArray());

                    return Result.EndPointResult(result,
                        Data.Succesfully,
                        Data.Fail);


                });


            }
        }


        public static Supplier Map(this SupplierResponse request, Supplier row)
        {
            row.Name = request.Name;

            row.SupplierCurrency = request.SupplierCurrency.Id;
            row.Address = request.Address;
            row.NickName = request.NickName;
            row.VendorCode = request.VendorCode;
            row.TaxCodeLP = request.TaxCodeLP;
            row.TaxCodeLD = request.TaxCodeLD;
            row.PhoneNumber = request.PhoneNumber;
            row.ContactEmail = request.ContactEmail;
            row.ContactName = request.ContactName;
            return row;
        }

    }

}
