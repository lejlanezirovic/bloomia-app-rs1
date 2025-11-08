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
        public GetClientByIdQueryValidator(IAppDbContext context) {
            RuleFor(x => x.ClientId).GreaterThan(0).WithMessage("ClientId must be a positive value");

            RuleFor(x=>x.ClientId).MustAsync(async (ClientId, ct) => await context.Clients.AnyAsync(x => x.Id == ClientId, ct))
            .WithMessage("Client with that id doesn't exist in database");
        }
        
    }
}
