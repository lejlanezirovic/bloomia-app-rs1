using Bloomia.Application.Modules.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetById
{
    public class GetClientByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<GetClientByIdQuery, GetClientByIdQueryDTO>
    {
        public async Task<GetClientByIdQueryDTO> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            if (!currentUser.IsAdmin)
            {
                var therapist = await context.Therapists.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, cancellationToken);

                var isTherapistsClient = therapist != null &&
                    await context.Appointments.AsNoTracking()
                        .IsClientOfTherapistAsync(therapist.Id, request.ClientId, cancellationToken);

                if (!isTherapistsClient)
                    throw new BloomiaNotFoundException("Client not found.");
            }

            var today = DateTime.Today;
            var client = await context.Clients.Include(x => x.User).Where(x => x.Id == request.ClientId).Select
                 (x => new GetClientByIdQueryDTO
                 {
                     ClientId = x.Id,
                     Firstname = x.User.Firstname,
                     Lastname = x.User.Lastname,
                     Age = x.User.DateOfBirth.HasValue? today.Year-x.User.DateOfBirth.Value.Year: 20,
                     Gender = x.User.Gender.Name,
                     City = x.User.Location.City,
                     Country = x.User.Location.Country,
                     NativeLanguage = x.User.Language.Name
                 }).FirstOrDefaultAsync(cancellationToken);

            if (client == null)
                throw new BloomiaNotFoundException("Client not found.");

            return client;
        }
    }
}
