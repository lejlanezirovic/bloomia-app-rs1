using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register
{
    public sealed class UserRegisterCommand: IRequest<UserRegisterCommandDto>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Firstname { get; init; }
        public string Lastname { get; init; }
        public string Username { get; init; }

        public string GenderName { get; init; }
        public string LocationCityName { get; init; }// city
        public string LocationCountryName { get; init; } // country
        public string LanguageName { get; init; }
        public DateTime? DateOfBirth { get; init; }

    }
}
