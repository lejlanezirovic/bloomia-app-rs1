using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Queries.List
{
    public sealed class ListMoodEntriesQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ListMoodEntriesQuery, PageResult<ListMoodEntriesQueryDto>>
    {
        public async Task<PageResult<ListMoodEntriesQueryDto>> Handle(ListMoodEntriesQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You must be logged in.");

            int clientId;
            var client = await context.Clients
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if(client != null)
            {
                clientId = client.Id;
            }
            else
            {
                if(request.ClientId == null)
                    throw new BloomiaBusinessRuleException("", "ClientId is required when logged in as therapist.");

                clientId = request.ClientId.Value;

                var hasAppointment = await context.Appointments
                    .Include(x => x.TherapistAvailability)
                        .ThenInclude(t => t.Therapist)
                    .AnyAsync(x => x.ClientId == clientId && x.TherapistAvailability.Therapist.UserId == currentUser.UserId, ct);

                if(!hasAppointment)
                    throw new BloomiaBusinessRuleException("", "You can only access your clients mood entries.");
            }

            var query = context.Moods
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.RecordedTime.Date)
                .Select(x => new ListMoodEntriesQueryDto
                {
                    Id = x.Id,
                    RecordedTime = x.RecordedTime,
                    Happiness = x.happiness,
                    Sadness = x.sadness,
                    Anger = x.anger,
                    Depression = x.depression,
                    Stress = x.stress,
                    Anxiety = x.anxiety,
                    WeekNumber = x.WeekNumber
                });

            return await PageResult<ListMoodEntriesQueryDto>
                .FromQueryableAsync(query, request.Paging, ct);
        }
    }
}
