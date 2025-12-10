using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Command.Delete
{
    public class DeleteAppointmentCommandValidation:AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentCommandValidation()
        {
            RuleFor(x=>x.AppointmentId).NotEmpty().WithMessage("Appointment Id is required.");
        }
    }
}
