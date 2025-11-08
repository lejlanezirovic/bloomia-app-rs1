using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetClientProfileById
{
    public class GetClientProfileByIdQuery: IRequest<GetClientProfileByIdQueryDTO>
    {
        public int UserId { get; set; }
    }
}
