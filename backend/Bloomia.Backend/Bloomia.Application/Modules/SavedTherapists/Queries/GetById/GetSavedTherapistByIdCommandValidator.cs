using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetById
{
    public class GetSavedTherapistByIdCommandValidator: AbstractValidator<GetSavedTherapistByIdCommand>
    {
        public GetSavedTherapistByIdCommandValidator()
        {
            RuleFor(x=>x.TherapistId).GreaterThan(0).WithMessage("ID terapeuta mora biti veci od 0");
            RuleFor(x=>x.TherapistId).NotEmpty().WithMessage("ID terapeuta je obavezan");
        }
    }
}
