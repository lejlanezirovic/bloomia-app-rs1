using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bloomia.Application.Modules.Users.Commands.UploadProfileImage
{
    public sealed class UploadProfileImageCommand : IRequest<UploadProfileImageCommandDto>
    {
        public IFormFile File { get; init; } = null!;

        [BindNever]
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
