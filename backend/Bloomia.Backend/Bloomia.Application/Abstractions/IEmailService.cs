using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface IEmailService
    {
        Task SendAppointmentBookingConfirmationAsync(string toEmail, string clientName, string therapistName,
            DateOnly appointmentDate, TimeOnly appointmentTime, string sessionType, CancellationToken ct); 
    }
}
