using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Reviews.Commands.Create
{
    public sealed class CreateReviewCommand : IRequest<int>
    {
        public int AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
