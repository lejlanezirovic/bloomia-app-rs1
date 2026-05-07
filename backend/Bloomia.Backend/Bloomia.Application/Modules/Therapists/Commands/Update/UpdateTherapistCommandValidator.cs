using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.Update
{
    public class UpdateTherapistCommandValidator : AbstractValidator<UpdateTherapistCommand>
    {
        public UpdateTherapistCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email!)
                .EmailAddress().WithMessage("Email invalid format");
            });
        }
    }
}
