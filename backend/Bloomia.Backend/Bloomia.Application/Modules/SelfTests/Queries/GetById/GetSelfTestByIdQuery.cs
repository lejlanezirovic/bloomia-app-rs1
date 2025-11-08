using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.GetById
{
    public class GetSelfTestByIdQuery:IRequest<GetSelfTestByIdQueryDto>
    {
        public int SelfTestId {  get; init; }
    }
}
