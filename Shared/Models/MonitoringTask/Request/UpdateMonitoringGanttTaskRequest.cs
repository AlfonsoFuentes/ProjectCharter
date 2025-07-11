using Shared.Enums.TaskStatus;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.FileResults.Generics.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MonitoringTask.Request
{
    public class UpdateMonitoringGanttTaskRequest : IRequest, IMessageResponse
    {
        public Guid GanttTaskId { get; set; }

        public string EndPointName => StaticClass.MonitoringTasks.EndPoint.CreateUpdate;

        public string Legend => "Update Tasks";

        public string ClassName => StaticClass.MonitoringTasks.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, "update");
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, "Update");
        public string? RealDurationUnit { get; set; } = string.Empty;
        public bool SetStartDate {  get; set; } = false;
        public double RealDurationInDays { get; set; } = 0;
        public double RealDurationInUnit { get; set; } = 0;
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public GanttTaskStatusEnum TaskStatus { get; set; } = GanttTaskStatusEnum.NotInitiated;
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
        public void CalculateDuration()
        {
            if (RealStartDate.HasValue && RealEndDate.HasValue)
            {
                RealDurationInDays = (RealEndDate.Value - RealStartDate.Value).TotalDays;
            }
            else
            {
                RealDurationInDays = 0;
            }
            switch (RealDurationUnit)
            {
                case "d":
                    RealDurationInUnit = RealDurationInDays;
                    break;
                case "w":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 7, 1);
                    break;
                case "m":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 30, 1);
                    break;
                case "y":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 365, 1);
                    break;
                case "":
                    RealDurationInUnit = RealDurationInDays;

                    break;
                default:
                    RealDurationInUnit = RealDurationInDays;

                    break;
            }
        }
        public void CalculateDurationWithEndDate(DateTime? enddate)
        {
            if (RealStartDate.HasValue && enddate.HasValue)
            {
                RealDurationInDays = (enddate.Value - RealStartDate.Value).TotalDays;
            }
            else
            {
                RealDurationInDays = 0;
            }
            switch (RealDurationUnit)
            {
                case "d":
                    RealDurationInUnit = RealDurationInDays;
                    break;
                case "w":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 7, 1);
                    break;
                case "m":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 30, 1);
                    break;
                case "y":
                    RealDurationInUnit = Math.Round(RealDurationInDays / 365, 1);
                    break;
                case "":
                    RealDurationInUnit = RealDurationInDays;

                    break;
                default:
                    RealDurationInUnit = RealDurationInDays;

                    break;
            }
        }
        public string? RealDuration
        {
            get
            {
                if (IsCalculated)
                {
                    this.CalculateDuration();
                }
                return this.GetDuration();

            }
            set
            {
                this.SetDuration(value);

            }
        }
        public bool IsCalculated { get; set; }
        public string GetDuration()
        {

            return $"{RealDurationInUnit}{RealDurationUnit}";
        }

        public void SetDuration(string? newduration)
        {
            var rawinput = newduration;
            if (string.IsNullOrWhiteSpace(rawinput))
            {
                rawinput = "1d";
            }
            var input = rawinput.Trim();

            var match = System.Text.RegularExpressions.Regex.Match(input, @"^(\d+)\s*([dwmy]?)$");
            if (!match.Success)
            {
                return;
            }
            if (!int.TryParse(match.Groups[1].Value, out var numericValue) || numericValue < 0)
            {
                return;
            }
            RealDurationInUnit = numericValue;

            var newdurationunits = match.Groups[2].Value.ToLower();
            if (string.IsNullOrEmpty(newdurationunits))
            {
                newdurationunits = RealDurationUnit;
            }
            RealDurationUnit = newdurationunits;

            switch (RealDurationUnit)
            {
                case "d":
                    RealDurationInDays = numericValue;
                    break;
                case "w":
                    RealDurationInDays = numericValue * 7;
                    break;
                case "m":
                    RealDurationInDays = numericValue * 30;
                    break;
                case "y":
                    RealDurationInDays = numericValue * 365;
                    break;
                case "":
                    RealDurationInDays = numericValue;

                    break;
                default:
                    RealDurationInDays = numericValue;

                    break;
            }


        }
    }
}
