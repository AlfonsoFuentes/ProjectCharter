using Shared.Models.MonitoringLogs.Requests;
using Shared.Models.MonitoringLogs.Responses;
using Shared.Models.MonitoringLogs.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.MonitoringLogs
{

    public class MonitoringLogValidator : AbstractValidator<MonitoringLogResponse>
    {
        private readonly IGenericService Service;

        public MonitoringLogValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.InitialDate).NotNull().When(x=>x.Id == Guid.Empty)
                .WithMessage("Initial date must defined");
            RuleFor(x => x.EndDate).NotNull().When(x => x.Id != Guid.Empty)
              .WithMessage("End date must defined");
            RuleFor(x => x.EndDate).NotEmpty().When(x => x.Id != Guid.Empty)
             .WithMessage("Closing text must defined");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");
           

        }

        async Task<bool> ReviewIfNameExist(MonitoringLogResponse request, string name, CancellationToken cancellationToken)
        {
            ValidateMonitoringLogRequest validate = new()
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
