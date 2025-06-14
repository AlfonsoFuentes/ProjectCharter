using FluentValidation;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.DeliverableGanttTasks.Validators;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.NewDeliverableGanttTaskRows
{
    public class DeliverableGanttTaskValidator : AbstractValidator<DeliverableGanttTaskResponse>
    {
        private readonly IGenericService Service;

        public DeliverableGanttTaskValidator(IGenericService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.StartDate).Must(ReviewStartDate).WithMessage("End Date must be Higher thant Start Date");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");
            RuleForEach(x => x.BudgetItemGanttTasks).ChildRules(order =>
            {
                order.RuleFor(x => x.PendingToAssign).GreaterThanOrEqualTo(0).When(order => order.BudgetItemId != Guid.Empty)
                .WithMessage("Pending Budget must be greater than zero");

                order.RuleFor(x => x.BudgetAssignedUSD).GreaterThan(0).When(order => order.BudgetItemId != Guid.Empty)
              .WithMessage("Pending Budget must be greater than zero");

                order.RuleFor(x => x.SelectedEngineeringItemsBudget).NotNull().When(order => order.BudgetItemId != Guid.Empty && order.BudgetItem!.HasSubItems)
                .WithMessage("Must define sub item");

            });
        }
        async Task<bool> ReviewIfNameExist(DeliverableGanttTaskResponse request, string name, CancellationToken cancellationToken)
        {

            ValidateDeliverableGanttTaskRequest validate = new()
            {
                Name = name,
                IsDeliverable = request.IsDeliverable,
                ProjectId = request.ProjectId,
                Id = request.Id == Guid.Empty ? null : request.Id,
                DeliverableId = request.DeliverableId,


            };
            var result = await Service.Validate(validate);
            return !result;
        }
        bool ReviewStartDate(DeliverableGanttTaskResponse request, DateTime? startdate)
        {
            if (request.IsDeliverable) return true;

            return request.StartDate != null && request.EndDate != null && startdate < request.EndDate;
        }
    }
}
