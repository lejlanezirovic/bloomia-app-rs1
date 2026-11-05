using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetById
{
    public sealed class GetClientByIdQueryValidator: AbstractValidator<GetClientByIdQuery>
    {
        public GetClientByIdQueryValidator() {
            RuleFor(x => x.ClientId).GreaterThan(0).WithMessage("ClientId must be a positive value");
        }
        
    }
}
