using FluentValidation;
using Shared.Enums.CurrencyEnums;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Validators;
using System.Xml.Linq;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.PurchaseOrders
{
    public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderRequest>
    {
        private readonly IGenericService Service;
        public CreatePurchaseOrderValidator(IGenericService _Service)
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
            RuleFor(x => x.CurrencyDate).NotNull().WithMessage("Currency date must be defined");

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
        bool ReviewNameExist(CreatePurchaseOrderRequest request, string name/*, CancellationToken cancellationToken*/)
        {
            //ValidatePurchaseOrderNameRequest validate = new()
            //{
            //    Name = request.Name,
            //    ProjectId = request.ProjectId,
            //};
            //var result = await Service.Validate(validate);
            bool result = request.ProjectPurchaseOrders.Any(x => x.Name.Equals(name));
            

            return !result;

           
        }
        //bool ReviewPRExist(CreatePurchaseOrderRequest request, string pr/*, CancellationToken cancellationToken*/)
        //{

        //    //ValidatePurchaseOrderRequisitionRequest validate = new()
        //    //{
        //    //    PurchaseRequisition = request.PurchaseRequisition,

        //    //};
        //    //var result = await Service.Validate(validate);
        //    bool result = request.ProjectPurchaseOrders.Any(x => x.PurchaseRequisition.Equals(pr));
        //    return !result;
        //}
        async Task<bool> ReviewPRExist(CreatePurchaseOrderRequest request, string pr, CancellationToken cancellationToken)
        {

            ValidatePurchaseOrderRequisitionRequest validate = new()
            {
                Id = request.Id,
                PurchaseRequisition = request.PurchaseRequisition,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }

}
