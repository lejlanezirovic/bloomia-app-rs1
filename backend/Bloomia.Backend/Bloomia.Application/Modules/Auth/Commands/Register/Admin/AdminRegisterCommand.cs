using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register.Admin
{
    public sealed class AdminRegisterCommand : IRequest<AdminRegisterCommandDto>
    {
        public string Email { get; set; }   
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
    }
}
