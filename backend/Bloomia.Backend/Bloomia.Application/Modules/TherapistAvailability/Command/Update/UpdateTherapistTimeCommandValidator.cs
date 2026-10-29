using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Update
{
    public class UpdateTherapistTimeCommandValidator:AbstractValidator<UpdateTherapistTimeCommand>
    {
        public UpdateTherapistTimeCommandValidator() {
            RuleFor(x => x.TherapistAvailabilityId).NotEmpty().WithMessage("TherapistAvailabilityId is requied field!");
            RuleFor(x => x.NewDate).NotEmpty().WithMessage("New date is requied field!");
            RuleFor(x => x.NewTime).NotEmpty().WithMessage("new time is requied field!");
            RuleFor(x => x.NewDate)
                .Must(d => d >= BusinessTime.Today)
                .WithMessage("New date must be today or a future date!");
            RuleFor(x => x.NewTime)
                .Must(SessionRules.IsWithinWorkingHours)
                .WithMessage(_ => SessionRules.WorkingHoursMessage);

            RuleFor(x => x.NewTime)
                .Must((command, time) => BusinessTime.ToUtc(command.NewDate, time) > DateTime.UtcNow)
                .WithMessage("The new scheduled time must be greater than the current time");
        }
    }
}
