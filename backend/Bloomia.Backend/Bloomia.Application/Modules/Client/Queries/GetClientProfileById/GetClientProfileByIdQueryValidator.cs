using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetClientProfileById
{
    public sealed class GetClientProfileByIdQueryValidator:AbstractValidator<GetClientProfileByIdQuery>
    {
        public GetClientProfileByIdQueryValidator() { 
        
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be a positive value");
        }
       
    }
}
