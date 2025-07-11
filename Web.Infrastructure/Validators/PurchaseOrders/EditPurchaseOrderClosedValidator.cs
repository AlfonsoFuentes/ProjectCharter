using Shared.Enums.CurrencyEnums;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.PurchaseOrders
{
    public class EditPurchaseOrderClosedValidator : AbstractValidator<EditPurchaseOrderClosedRequest>
    {
        private readonly IGenericService Service;
        public EditPurchaseOrderClosedValidator(IGenericService _Service)
        {
            Service = _Service;

            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");


            RuleFor(X => X.Name).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.Name).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition)
                 .Must(x => x.StartsWith("PR"))
                 .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition))
                 .WithMessage("PR must include PR letter at start");

            RuleFor(X => X.PONumber).NotEmpty().WithMessage("PO number must be defined");
            RuleFor(X => X.PONumber).NotNull().WithMessage("PO number must be defined");

            RuleFor(X => X.PONumber)
              .Must(x => x.StartsWith("850"))
              .When(x => !string.IsNullOrEmpty(x.PONumber))
              .WithMessage("PO must start with 850");

            RuleFor(x => x.PONumber).MustAsync(ReviewPOExist)
               .When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage(x => $"{x.PONumber} already exist");

            RuleFor(x => x.PONumber)
               .Length(10)
               .When(x => x.PONumber.StartsWith("850"))
               .WithMessage("PO number must have 10 characters");
            RuleFor(x => x.PONumber)
                .Matches("^[0-9]*$")
                .When(x => x.PONumber.Length == 10)
                .WithMessage("PO Number must be number!");

            RuleFor(x => x.ExpectedDate).NotNull().WithMessage("Expected date must be defined");

            RuleFor(x => x.CurrencyDate).NotNull().WithMessage("Currency date must be defined");
            RuleFor(x => x.ReceivingDate).NotNull().WithMessage("Receiving date must be defined");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");

            RuleFor(x => x.TotalUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item must have Currency value greater Than zero");
            RuleFor(x => x.IsAnyNameEmpty).NotEqual(true).WithMessage("All purchase orders items must have a Name");

            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");

            //RuleFor(x => x.Name).Must(ReviewNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(x => $"{x.Name} already exist"); ;

            RuleFor(x => x.PurchaseRequisition).MustAsync(ReviewPRExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseRequisition} already exist");

            RuleFor(x => x.IsAnyPendingToReceiveLessThanZero).Equal(false).WithMessage("Must equal or les than purchase order value");
            RuleForEach(x => x.PurchaseOrderItemReceiveds).ChildRules(order =>
            {
                order.RuleFor(x => x.ValueReceivedCurrency).GreaterThan(0).WithMessage("Required");


            });
            RuleForEach(x => x.SelectedPurchaseOrderItems).ChildRules(order =>
            {
                order.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Required");
                order.RuleFor(x => x.UnitaryQuoteCurrency).GreaterThan(0).WithMessage("Required");
                order.RuleFor(x => x.BudgetItemId).NotEqual(Guid.Empty).WithMessage("Required");
                order.RuleFor(x => x.Name).NotEmpty().WithMessage("Required");
                order.RuleFor(x => x.Name).NotNull().WithMessage("Required");
                //order.RuleFor(x => x.BasicResponse).NotNull().When(x => x.BudgetItem != null & x.BudgetItem!.HasSubItems).WithMessage("Required");
            });

        }
        bool ReviewNameExist(EditPurchaseOrderClosedRequest request, string name/*, CancellationToken cancellationToken*/)
        {
            //ValidatePurchaseOrderNameRequest validate = new()
            //{
            //    Name = request.Name,
            //    ProjectId = request.ProjectId,
            //};
            //var result = await Service.Validate(validate);
            bool result = request.ProjectPurchaseOrders.Any(x => x.Id != request.Id && x.Name.Equals(name));


            return !result;


        }
        //bool ReviewPRExist(EditPurchaseOrderClosedRequest request, string pr/*, CancellationToken cancellationToken*/)
        //{

        //    //ValidatePurchaseOrderRequisitionRequest validate = new()
        //    //{
        //    //    PurchaseRequisition = request.PurchaseRequisition,

        //    //};
        //    //var result = await Service.Validate(validate);
        //    bool result = request.ProjectPurchaseOrders.Any(x => x.Id != request.Id && x.PurchaseRequisition.Equals(pr));
        //    return !result;
        //}
        //bool ReviewPOExist(EditPurchaseOrderClosedRequest request, string po/*, CancellationToken cancellationToken*/)
        //{

        //    //ValidatePurchaseOrderRequisitionRequest validate = new()
        //    //{
        //    //    PurchaseRequisition = request.PurchaseRequisition,

        //    //};
        //    //var result = await Service.Validate(validate);
        //    bool result = request.ProjectPurchaseOrders.Any(x => x.Id != request.Id && x.PONumber.Equals(po));
        //    return !result;
        //}
        //async Task<bool> ReviewNameExist(EditPurchaseOrderClosedRequest request, string name, CancellationToken cancellationToken)
        //{
        //    ValidatePurchaseOrderNameRequest validate = new()
        //    {
        //        Id = request.Id,
        //        Name = request.Name,
        //        ProjectId = request.ProjectId,
        //    };
        //    var result = await Service.Validate(validate);


        //    return !result;
        //}
        async Task<bool> ReviewPRExist(EditPurchaseOrderClosedRequest request, string pr, CancellationToken cancellationToken)
        {

            ValidatePurchaseOrderRequisitionRequest validate = new()
            {
                Id = request.Id,
                PurchaseRequisition = request.PurchaseRequisition,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        async Task<bool> ReviewPOExist(EditPurchaseOrderClosedRequest request, string pr, CancellationToken cancellationToken)
        {

            ValidatePurchaseOrderNumberRequest validate = new()
            {
                Id = request.Id,
                Number = request.PONumber,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
        //bool PendingToReceive(PurchaseOrderItemRequest request, double value)
        //{
        //    if (value < 0)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }

}
