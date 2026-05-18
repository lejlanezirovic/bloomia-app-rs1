using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Abstractions
{
    public interface IAppointmentReminderService
    {
        Task ProcessTomorrowRemindersAsync(CancellationToken ct);
    }
}
