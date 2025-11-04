using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Therapist
{
    public sealed class TherapistRegisterCommand : IRequest<TherapistRegisterCommandDto>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Firstname { get; init; }
        public string Lastname { get; init; }
        public string Username { get; init; }
        public string Specialization { get; init; }
        public string Description { get; init; }
        public int DocumentId { get; init; }

    }
}
