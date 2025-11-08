using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetById
{
    public class GetClientByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetClientByIdQuery, GetClientByIdQueryDTO>
    {
        public async Task<GetClientByIdQueryDTO> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
           var today = DateTime.Today;
            var client = await context.Clients.Include(x => x.User).Where(x => x.Id == request.ClientId).Select
                 (x => new GetClientByIdQueryDTO
                 {
                     ClientId = x.Id,
                     Firstname = x.User.Firstname,
                     Lastname = x.User.Lastname,
                     Username = x.User.Username,
                     Age = x.User.DateOfBirth.HasValue? today.Year-x.User.DateOfBirth.Value.Year: 20,
                     Gender = x.User.Gender.Name,
                     City = x.User.Location.City,
                     Country = x.User.Location.Country,
                     NativeLanguage = x.User.Language.Name
                 }).FirstOrDefaultAsync(cancellationToken);

            if (client == null)
                throw new ValidationException(message: $"Client with id {request.ClientId} doesn't exist in database");

            return client;
        }
    }
}
