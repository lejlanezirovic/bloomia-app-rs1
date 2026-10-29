using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapistAvailability.Command.Delete.DeleteTimeByDate
{
    public class DeleteTherapistAvailableTimeByDateCommandValidator:AbstractValidator<DeleteTherapistAvailableTimeByDateCommand>
    {
        public DeleteTherapistAvailableTimeByDateCommandValidator()
        {
            RuleFor(x => x.Date).Must(d => d >= BusinessTime.Today)
                .WithMessage("You cannot delete an appointment that has passed");

            RuleFor(x => x.TimeToDelete)
                .Must((command, time) => BusinessTime.ToUtc(command.Date, time) > DateTime.UtcNow)
                .WithMessage("The time to delete must be greater than the current time!");
        }
    }
}
