using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.PurchaseOrders
{
    public class EditSalaryPurchaseOrderValidator : AbstractValidator<EditPurchaseOrderSalaryRequest>
    {
        private readonly IGenericService Service;
        public EditSalaryPurchaseOrderValidator(IGenericService _Service)
        {
            Service = _Service;

            RuleFor(X => X.Name).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.Name).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(x => x.ValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

       
        RuleFor(x => x.Name).Must(ReviewNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist"); ;

            RuleFor(X => X.PONumber)
              .Must(x => x.StartsWith("850"))
              .When(x => !string.IsNullOrEmpty(x.PONumber))
              .WithMessage("PO must start with 850");

            RuleFor(x => x.PONumber).MustAsync(ReviewPOExist)
               .When(x => !string.IsNullOrEmpty(x.PONumber))
               .WithMessage(x => $"{x.PONumber} already exist");

            RuleFor(x => x.PONumber)
               .Length(10)
               .When(x => !string.IsNullOrEmpty(x.PONumber) && x.PONumber.StartsWith("850"))
               .WithMessage("PO number must have 10 characters");
            RuleFor(x => x.PONumber)
                .Matches("^[0-9]*$")
                .When(x => !string.IsNullOrEmpty(x.PONumber) && x.PONumber.Length == 10)
                .WithMessage("PO Number must be number!");

            RuleFor(x => x.SalaryDate).NotNull().WithMessage("Salary date must be defined");
        }
        bool ReviewNameExist(EditPurchaseOrderSalaryRequest request, string name/*, CancellationToken cancellationToken*/)
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
        bool ReviewPRExist(EditPurchaseOrderSalaryRequest request, string pr/*, CancellationToken cancellationToken*/)
        {

            //ValidatePurchaseOrderRequisitionRequest validate = new()
            //{
            //    PurchaseRequisition = request.PurchaseRequisition,

            //};
            //var result = await Service.Validate(validate);
            bool result = request.ProjectPurchaseOrders.Any(x => x.Id != request.Id && x.PurchaseRequisition.Equals(pr));
            return !result;
        }
        //bool ReviewPOExist(CreateSalaryPurchaseOrderRequest request, string po/*, CancellationToken cancellationToken*/)
        //{

        //    //ValidatePurchaseOrderRequisitionRequest validate = new()
        //    //{
        //    //    PurchaseRequisition = request.PurchaseRequisition,

        //    //};
        //    //var result = await Service.Validate(validate);
        //    bool result = request.ProjectPurchaseOrders.Any(x => x.Id != request.Id && x.PONumber.Equals(po));
        //    return !result;
        //}
        //async Task<bool> ReviewNameExist(CreateSalaryPurchaseOrderRequest request, string name, CancellationToken cancellationToken)
        //{
        //    ValidatePurchaseOrderNameRequest validate = new()
        //    {
        //        Name = request.Name,
        //        ProjectId = request.ProjectId,
        //    };
        //    var result = await Service.Validate(validate);


        //    return !result;
        //}
        async Task<bool> ReviewPOExist(EditPurchaseOrderSalaryRequest request, string pr, CancellationToken cancellationToken)
        {

            ValidatePurchaseOrderNumberRequest validate = new()
            {
                Id = request.Id,
                Number = request.PONumber,

            };
            var result = await Service.Validate(validate);
            return !result;
        }

    }

}
