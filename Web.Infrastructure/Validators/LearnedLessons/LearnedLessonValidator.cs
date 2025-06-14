using Shared.Models.LearnedLessons.Responses;
using Shared.Models.LearnedLessons.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.LearnedLessons
{
  
    public class LearnedLessonValidator : AbstractValidator<LearnedLessonResponse>
    {
        private readonly IGenericService Service;

        public LearnedLessonValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(LearnedLessonResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateLearnedLessonRequest validate = new()
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
