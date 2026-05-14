using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Commands.UploadProfileImage
{
    public sealed class UploadProfileImageCommandDto
    {
        public string Note { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
    }
}
