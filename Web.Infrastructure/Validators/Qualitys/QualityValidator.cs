using Shared.Models.Qualitys.Responses;
using Shared.Models.Qualitys.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Qualitys
{
  
    public class QualityValidator : AbstractValidator<QualityResponse>
    {
        private readonly IGenericService Service;

        public QualityValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(QualityResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateQualityRequest validate = new()
            {
                Name = name,

                ProjectId = request.ProjectId,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
