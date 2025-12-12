using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.MoodsFolder;

namespace Bloomia.Application.Modules.Moods.Commands.Create
{
    public sealed class CreateMoodEntryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<CreateMoodEntryCommand, int>
    {
        public async Task<int> Handle(CreateMoodEntryCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");
            
            var client = await context.Clients
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (client == null)
                throw new BloomiaNotFoundException("Client not found.");

            var today = DateTime.UtcNow.Date;

            if (today.DayOfWeek != DayOfWeek.Sunday)
                throw new BloomiaBusinessRuleException("", "Mood-tracker can only be completed on sunday.");


            var registrationDate = client.CreatedAtUtc.Date;
            var totalDays = (today - registrationDate).TotalDays;
            var weekNumber = (int)(totalDays / 7) + 1;

            var moodEntryExists = await context.Moods
                .AnyAsync(x => x.ClientId == client.Id &&
                        x.WeekNumber == weekNumber, ct);

            if (moodEntryExists)
                throw new BloomiaBusinessRuleException("", "Mood entry for this week has already been recorded.");


            var moodEntry = new MoodEntity
            {
                ClientId = client.Id,
                RecordedTime = DateTime.UtcNow,
                happiness = request.Happiness,
                sadness = request.Sadness,
                anger = request.Anger,
                stress = request.Stress,
                depression = request.Depression,
                anxiety = request.Anxiety,
                WeekNumber = weekNumber,
            };

            context.Moods.Add(moodEntry);
            await context.SaveChangesAsync(ct);

            return moodEntry.Id;
        }
    }
}
