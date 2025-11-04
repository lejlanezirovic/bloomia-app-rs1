using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Therapist
{
    public sealed class TherapistRegisterCommandValidator : AbstractValidator<TherapistRegisterCommand>
    {
        public TherapistRegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                                 .EmailAddress().WithMessage("Email invalid format")
                                 .Matches(@"@(gmail\.com|yahoo\.com|edu\.)$")
                                 .WithMessage("Email domain must me @gmail, @yahoo or @edu ");
            
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname is required").MaximumLength(50);

            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname is required").MaximumLength(50);

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required").MaximumLength(30);

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.DocumentId)
                .GreaterThan(0).WithMessage("Neispravan ID dokumenta.");
        }
    }
}
