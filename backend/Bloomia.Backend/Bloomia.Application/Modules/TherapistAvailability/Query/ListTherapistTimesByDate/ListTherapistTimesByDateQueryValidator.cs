using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.TherapistAvailability.Query.ListTherapistTimesByDate;

namespace Bloomia.Application.Modules.TherapistAvailability.Query.ListAllTimesByDate
{
    public class ListTherapistTimesByDateQueryValidator:AbstractValidator<ListTherapistTimesByDateQuery>
    {
        public ListTherapistTimesByDateQueryValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is mandatory!");
        }
    }
}
