using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Users.Queries.List
{
    public sealed class ListUsersQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ListUsersQuery, PageResult<ListUsersQueryDto>>
    {
        public async Task<PageResult<ListUsersQueryDto>> Handle(ListUsersQuery request, CancellationToken ct)
        {
            if (currentUser.UserId == null || !currentUser.IsAdmin)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only an admin can retrieve the list of all users.");

            var query = context.Users
                .AsNoTracking()
                .Include(x => x.Role).AsQueryable();

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x => x.Firstname.ToLower().Contains(request.Search.ToLower())
                    || x.Lastname.ToLower().Contains(request.Search.ToLower())
                    || (x.Email != null && x.Email.Contains(request.Search))
                    || (x.Username != null && x.Username.Contains(request.Search)));
            }

            if(request.OnlyEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == request.OnlyEnabled);
            }

            if(request.Role != null)
            {
                query = query.Where(x => x.Role.RoleName == request.Role);
            }

            var projectedQuery = query.OrderBy(x => x.Firstname)
                .Select(x => new ListUsersQueryDto
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Fullname = (x.Firstname + " " + x.Lastname),
                    Username = x.Username,
                    Email = x.Email,
                    Role = x.Role.RoleName,
                    IsEnabled = x.IsEnabled
                });

            return await PageResult<ListUsersQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
