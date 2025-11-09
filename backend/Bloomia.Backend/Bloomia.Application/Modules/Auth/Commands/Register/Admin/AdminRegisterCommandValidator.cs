using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Admin
{
    public sealed class AdminRegisterCommandValidator : AbstractValidator<AdminRegisterCommand>
    {
        public AdminRegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email invalid format")
                .Matches(@"@(gmail\.com|yahoo\.com|edu\.)$")
                .WithMessage("Email domain must be @gmail, @yahoo or @edu");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("Firstname is required")
                .MaximumLength(50);

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Lastname is required")
                .MaximumLength(50);

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(30);
        }
    }
}
