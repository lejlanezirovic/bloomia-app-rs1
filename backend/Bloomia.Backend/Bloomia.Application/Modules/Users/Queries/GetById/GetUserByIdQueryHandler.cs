using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Queries.GetById
{
    public sealed class GetUserByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser):IRequestHandler<GetUserByIdQuery, GetUserByIdQueryDto>
    {
        public async Task<GetUserByIdQueryDto> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            if (!currentUser.IsAdmin && currentUser.UserId != request.Id)
                throw new BloomiaNotFoundException($"User with {request.Id} not found.");

            var user = await context.Users.AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (user == null)
            {
                throw new BloomiaNotFoundException($"User with {request.Id} not found.");
            }

            var userDto = new GetUserByIdQueryDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Fullname = user.Fullname,
                Email = user.Email,
                Role = user.Role.RoleName,
                IsEnabled = user.IsEnabled
            };

            return userDto;
        }
    }
}
