using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Create
{
    public class CreateTherapistAvailabilityCommandValidator:AbstractValidator<CreateTherapistAvailabilityCommand>
    {
        public CreateTherapistAvailabilityCommandValidator() {

            RuleFor(x => x.AvailableDate).NotNull().WithMessage("Must enter the AvailableDate field!");
            RuleFor(x => x.StartTime).NotNull().WithMessage("Must enter the StartTime field!");

            RuleFor(x => x.AvailableDate)
                .Must(d => d >= BusinessTime.Today)
                .WithMessage("Available date must be today or a future date!");

            RuleFor(x => x.StartTime)
                .Must(SessionRules.IsWithinWorkingHours)
                .WithMessage(_ => SessionRules.WorkingHoursMessage);

            RuleFor(x => x.StartTime)
                .Must((command, startTime) => BusinessTime.ToUtc(command.AvailableDate, startTime) > DateTime.UtcNow)
                .WithMessage("The scheduled time must be greater than the current time");
        }
    }

}
