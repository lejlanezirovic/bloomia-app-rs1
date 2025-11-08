using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.List
{
    public sealed class ListClientsQueryHandler(IAppDbContext context) : IRequestHandler<ListClientsQuery, PageResult<ListClientsQueryDto>>
    {
        public async Task<PageResult<ListClientsQueryDto>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = context.Clients.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            { 
                clients = clients.Where(x => x.User.Fullname.Contains(request.Search));                   
            }

            var clientsQuery = clients.OrderBy(x => x.User.Fullname)
                    .Select(x => new ListClientsQueryDto
                    {
                        Id = x.Id,
                        Fullname = x.User.Fullname,
                        IsEnabled = x.User.IsEnabled,
                        Email = x.User.Email,
                        CreatedAt = x.CreatedAtUtc
                    });

            return await PageResult<ListClientsQueryDto>.FromQueryableAsync(clientsQuery,request.Paging, cancellationToken);
        }
    }
}
