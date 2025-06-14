using Shared.Models.MeetingAgreements.Requests;
using Shared.Models.MeetingAgreements.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.MeetingAgreements
{
    public class CreateMeetingAgreementValidator : AbstractValidator<CreateMeetingAgreementRequest>
    {
        private readonly IGenericService Service;

        public CreateMeetingAgreementValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

           
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(CreateMeetingAgreementRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateMeetingAgreementRequest validate = new()
            {
                Name = name,
                MeetingId = request.MeetingId,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
    public class UpdateMeetingAgreementValidator : AbstractValidator<UpdateMeetingAgreementRequest>
    {
        private readonly IGenericService Service;

        public UpdateMeetingAgreementValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(UpdateMeetingAgreementRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateMeetingAgreementRequest validate = new()
            {
                Name = name,
                MeetingId = request.MeetingId,
                Id = request.Id

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
