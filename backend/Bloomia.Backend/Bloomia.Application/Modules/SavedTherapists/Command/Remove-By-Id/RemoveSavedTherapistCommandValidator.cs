using Bloomia.Application.Modules.SavedTherapists.Command.Remove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Remove_By_Id
{
    public class RemoveSavedTherapistCommandValidator:AbstractValidator<RemoveSavedTherapistCommand>
    {
        public RemoveSavedTherapistCommandValidator()
        {
            RuleFor(x => x.TherapistId).NotEmpty().WithMessage("Id terapeuta je obavezan!");
        }
    }
}
