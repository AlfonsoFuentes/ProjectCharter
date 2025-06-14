using Shared.Models.MeetingAgreements.Requests;
using Shared.Models.MeetingAgreements.Validators;
using Shared.Models.MeetingAttendants.Requests;
using Shared.Models.MeetingAttendants.Validators;
using Shared.Models.StakeHolders.Responses;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.MeetingAttendants
{
    public class CreateMeetingAttendantValidator : AbstractValidator<CreateMeetingAttendantRequest>
    {
        private readonly IGenericService Service;

        public CreateMeetingAttendantValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.StakeHolder).NotNull().WithMessage("StakeHolder must be defined!");


            RuleFor(x => x.StakeHolder).MustAsync(ReviewIfStakeHolderExist)
               .When(x => x.StakeHolder != null)
               .WithMessage(x => $"{x.StakeHolderName} already exist in Meeting");
        }
        async Task<bool> ReviewIfStakeHolderExist(CreateMeetingAttendantRequest request, StakeHolderResponse? name, CancellationToken cancellationToken)
        {
            ValidateMeetingAttendantRequest validate = new()
            {
                StakeHolderId = request.StakeHolder!.Id,
                MeetingId = request.MeetingId,

            };
            var result = await Service.Validate(validate);
            return !result;
        }

    }
    public class UpdateMeetingAttendantValidator : AbstractValidator<UpdateMeetingAttendantRequest>
    {
        private readonly IGenericService Service;

        public UpdateMeetingAttendantValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.StakeHolder).NotNull().WithMessage("StakeHolder must be defined!");
            RuleFor(x => x.StakeHolder).MustAsync(ReviewIfStakeHolderExist)
              .When(x => x.StakeHolder != null)
              .WithMessage(x => $"{x.StakeHolderName} already exist in Meeting");
        }

        async Task<bool> ReviewIfStakeHolderExist(UpdateMeetingAttendantRequest request, StakeHolderResponse? name, CancellationToken cancellationToken)
        {
            ValidateMeetingAttendantRequest validate = new()
            {
                StakeHolderId = request.StakeHolder!.Id,
                MeetingId = request.MeetingId,

            };
            var result = await Service.Validate(validate);
            return !result;
        }
    }
}
