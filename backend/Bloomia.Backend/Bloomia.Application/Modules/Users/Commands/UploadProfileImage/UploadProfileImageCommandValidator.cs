using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Commands.UploadProfileImage
{
    public sealed class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
    {
        public UploadProfileImageCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Profile image is required.");
        }
    }
}
