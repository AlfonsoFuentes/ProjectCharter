using Shared.Models.FinishingLines.LineSpeeds;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.FinishinLines.LineSpeeds
{
   
    public class LineSpeedValidator : AbstractValidator<LineSpeedResponse>
    {
        private readonly IGenericService Service;

        public LineSpeedValidator(IGenericService service)
        {
            Service = service;
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.MaxSpeedValue).GreaterThan(0).WithMessage("Max Speed must be defined!");
            RuleFor(x => x.Sku).NotNull().WithMessage("SKU must be defined!");
            RuleFor(x => x.PercentageAU).GreaterThan(0).WithMessage("AU% must be defined!");
            RuleFor(x => x.PercentageAU).LessThan(100).WithMessage("AU% must be less than 100%");
            //RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
            //    .When(x => !string.IsNullOrEmpty(x.Name))
            //    .WithMessage(x => $"{x.Name} already exist");

        }

        //async Task<bool> ReviewIfNameExist(LineSpeedResponse request, string name, CancellationToken cancellationToken)
        //{
        //    ValidateLineSpeedNameRequest validate = new()
        //    {
        //        Name = name,

        //        Id = request.Id

        //    };
        //    var result = await Service.Validate(validate);
        //    return !result;
        //}
    }
}
