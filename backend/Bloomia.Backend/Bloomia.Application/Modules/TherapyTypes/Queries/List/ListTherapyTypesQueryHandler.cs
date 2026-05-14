using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.TherapyTypes.Queries.List
{
    public sealed class ListTherapyTypesQueryHandler(IAppDbContext context) : IRequestHandler<ListTherapyTypesQuery, List<ListTherapyTypesQueryDto>>
    {
        public async Task<List<ListTherapyTypesQueryDto>> Handle(ListTherapyTypesQuery request, CancellationToken ct)
        {
            return await context.TherapyTypes
                .AsNoTracking()
                .OrderBy(x => x.TherapyName)
                .Select(x => new ListTherapyTypesQueryDto
                {
                    Id = x.Id,
                    Name = x.TherapyName,
                    Description = x.Description
                }).ToListAsync(ct);
        }
    }
}
