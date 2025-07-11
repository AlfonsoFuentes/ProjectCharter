using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.DeliverableGanttTasks.Validators;
using Shared.Models.MonitoringTask.Request;
using Web.Infrastructure.Managers.Generic;

namespace Web.Infrastructure.Validators.NewDeliverableGanttTaskRows
{
    public class UpdateMonitoringGanttTaskValidator : AbstractValidator<UpdateMonitoringGanttTaskRequest>
    {
       

        public UpdateMonitoringGanttTaskValidator()
        {
            
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be defined!");
            RuleFor(x => x.RealStartDate).Must(ReviewStartDate).WithMessage("End Date must be Higher thant Start Date");


            RuleFor(x => x.RealStartDate).NotNull().WithMessage("Start Date must be defined");
            RuleFor(x => x.RealEndDate).NotNull().WithMessage("End Date must be defined");
        }
       
        bool ReviewStartDate(UpdateMonitoringGanttTaskRequest request, DateTime? startdate)
        {
           

            return request.RealStartDate != null && request.RealEndDate != null && startdate < request.RealEndDate;
        }
    }
}
