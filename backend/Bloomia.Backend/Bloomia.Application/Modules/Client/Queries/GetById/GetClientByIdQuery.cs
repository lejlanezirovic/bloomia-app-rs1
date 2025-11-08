using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetById
{
    public class GetClientByIdQuery: IRequest<GetClientByIdQueryDTO>
    {
        public int ClientId { get; set; }
    }
}
