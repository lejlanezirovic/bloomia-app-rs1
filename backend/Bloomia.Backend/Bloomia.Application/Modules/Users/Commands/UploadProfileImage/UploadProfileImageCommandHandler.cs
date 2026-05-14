using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Commands.UploadProfileImage
{
    public sealed class UploadProfileImageCommandHandler(IAppDbContext context, IFileStorageService fileStorageService) : IRequestHandler<UploadProfileImageCommand, UploadProfileImageCommandDto>
    {
        public async Task<UploadProfileImageCommandDto> Handle(UploadProfileImageCommand request, CancellationToken ct)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId && !x.IsDeleted && x.IsEnabled, ct);

            if (user == null)
                throw new BloomiaNotFoundException("User not found.");

            var oldProfileImage = user.ProfileImage;

            var relativePath = await fileStorageService.SaveProfileImageAsync(request.File, ct);

            user.ProfileImage = relativePath;
            user.ModifiedAtUtc = DateTime.UtcNow;

            await context.SaveChangesAsync(ct);

            if (!string.IsNullOrWhiteSpace(oldProfileImage))
                fileStorageService.DeleteIfExists(oldProfileImage);

            var profileImageDto = new UploadProfileImageCommandDto
            {
                Note = "Profile image uploaded successfully.",
                ProfileImage = relativePath
            };

            return profileImageDto;
        }
    }
}
