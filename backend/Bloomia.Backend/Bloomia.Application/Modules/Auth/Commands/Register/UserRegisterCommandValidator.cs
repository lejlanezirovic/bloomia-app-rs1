using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register
{
    public sealed class UserRegisterCommandValidator: AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterCommandValidator() {

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                                 .EmailAddress().WithMessage("Email invalid format")
                                 .Matches(@"@(gmail\.com|yahoo\.com|edu\.)$")
                                 .WithMessage("Email domain must me @gmail, @yahoo or @edu ");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Passwordmust contain at least one number");
                

            RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname is required").MaximumLength(50);

            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname is required").MaximumLength(50);

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required").MaximumLength(30);
            //htjela bih za gender da stoji MALE FEMALE OTHER kao placeholder
            //RuleFor(x=>x.GenderName).NotEmpty().WithMessage("Gender field is required");

            //RuleFor(x => x.LocationCityName).NotEmpty().WithMessage("Location city is required");

            //RuleFor(x => x.LocationCountryName).NotEmpty().WithMessage("Location country is required");

            //RuleFor(x => x.LanguageName).NotEmpty().WithMessage("Language is required");
          
           //exposati exceptione

        }
    }
}
