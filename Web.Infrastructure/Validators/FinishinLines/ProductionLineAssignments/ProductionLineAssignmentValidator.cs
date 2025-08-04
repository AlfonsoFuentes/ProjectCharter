using Shared.Models.FinishingLines.ProductionLineAssignments;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.ProductionLineAssignments
{
   
    public class ProductionLineAssignmentValidator : AbstractValidator<ProductionLineAssignmentResponse>
    {
        private readonly IGenericService Service;

        public ProductionLineAssignmentValidator(IGenericService service)
        {
            Service = service;
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
       
            RuleFor(x => x.ProductionLine).NotNull().WithMessage("Line must be defined!");
            RuleFor(x => x.ScheduleItems.Count).GreaterThan(0).WithMessage("Schedule items must be defined!");
            //RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name))
            //    .WithMessage(x => $"{x.Name} already exist");

        }

        //async Task<bool> ReviewIfNameExist(ProductionLineAssignmentResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidateProductionLineAssignmentNameRequest validate = new()
        //    {
        //        Name = name,

        //        Id = request.Id

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
    }
}
