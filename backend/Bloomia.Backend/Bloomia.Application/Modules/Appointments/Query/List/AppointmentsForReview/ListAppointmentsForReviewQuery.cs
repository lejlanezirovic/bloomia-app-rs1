using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Appointments.Query.List.AppointmentsForReview
{
    public sealed class ListAppointmentsForReviewQuery : IRequest<List<ListAppointmentsForReviewQueryDto>>
    {
        public int TherapistId { get; set; }
    }
}
