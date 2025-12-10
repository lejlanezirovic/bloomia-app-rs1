using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Create
{
    public class CreateAppointmentCommandValidation:AbstractValidator<CreateAppoinmentCommand>
    {
        public CreateAppointmentCommandValidation()
        {
            RuleFor(x=>x.TherapistAvailabilityId).NotEmpty().WithMessage("Therapist Availability Id is required.");
            RuleFor(x=>x.SessionType).IsInEnum().WithMessage("Session Type is invalid.");
         
        }
    }
}
