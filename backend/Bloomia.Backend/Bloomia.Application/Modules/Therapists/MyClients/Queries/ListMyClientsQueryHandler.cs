using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.MyClients.Queries
{
    public class ListMyClientsQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<ListMyClientsQuery, PageResult<ListMyClientsQueryDto>>
    {
        public async Task<PageResult<ListMyClientsQueryDto>> Handle(ListMyClientsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            if (userId == null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            var therapist = await context.Therapists
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found.");

            var appointments = context.Appointments
                .AsNoTracking()
                .Where(x => x.TherapistAvailability.TherapistId == therapist.Id);

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                appointments = appointments.Where(x =>
                            x.Client.User.Fullname.ToLower().Contains(request.Search.ToLower()) ||
                            x.Client.User.Email.Contains(request.Search));
            }

            var projectedQuery = appointments
                .GroupBy(x => new
                {
                    x.ClientId,
                    x.Client.User.Fullname,
                    x.Client.User.Email,
                    x.Client.User.ProfileImage
                })
                .Select(g => new ListMyClientsQueryDto
                {
                    ClientId = g.Key.ClientId,
                    FullName = g.Key.Fullname ?? string.Empty,
                    Email = g.Key.Email,
                    ProfileImage = g.Key.ProfileImage,

                    NextAppointmentAtUtc = g
                    .Where(x => x.ScheduledAtUtc > DateTime.UtcNow)
                    .OrderBy(x => x.ScheduledAtUtc)
                    .Select(x => (DateTime?)x.ScheduledAtUtc)
                    .FirstOrDefault()
                })
                .OrderBy(x => x.FullName);

            var result =  await PageResult<ListMyClientsQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
        
            foreach(var client in result.Items)
            {
                if(client.NextAppointmentAtUtc.HasValue)
                {
                    client.NextAppointmentAtUtc = DateTime.SpecifyKind(client.NextAppointmentAtUtc.Value, DateTimeKind.Utc);
                }
            }

            return result;
        }
    }
}
