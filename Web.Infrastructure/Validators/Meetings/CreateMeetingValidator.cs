using Shared.Enums.Meetings;
using Shared.Models.Meetings.Requests;
using Shared.Models.Meetings.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.Meetings
{
    public class CreateMeetingValidator : AbstractValidator<CreateMeetingRequest>
    {
        private readonly IGenericService Service;

        public CreateMeetingValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

            RuleFor(x => x.MeetingType).NotEqual(MeetingTypeEnum.None).WithMessage("Type must be defined!");

            RuleFor(x => x.DateofMeeting).NotNull().WithMessage("Date of meeting must be defined!");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(CreateMeetingRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateMeetingRequest validate = new()
            {
                Name = name,
                ProjectId = request.ProjectId,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
    public class UpdateMeetingValidator : AbstractValidator<UpdateMeetingRequest>
    {
        private readonly IGenericService Service;

        public UpdateMeetingValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");

            RuleFor(x => x.MeetingType).NotEqual(MeetingTypeEnum.None).WithMessage("Type must be defined!");

            RuleFor(x => x.DateofMeeting).NotNull().WithMessage("Date of meeting must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(UpdateMeetingRequest request, string name, CancellationToken cancellationToken)
        {
            ValidateMeetingRequest validate = new()
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
