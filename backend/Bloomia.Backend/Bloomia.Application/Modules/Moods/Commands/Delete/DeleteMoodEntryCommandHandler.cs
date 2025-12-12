using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Commands.Delete
{
    public sealed class DeleteMoodEntryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
    : IRequestHandler<DeleteMoodEntryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteMoodEntryCommand request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");

            var client = await context.Clients
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (client == null)
                throw new BloomiaNotFoundException("Client not found.");

            var moodEntryToDelete = await context.Moods
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.ClientId == client.Id, ct);

            if (moodEntryToDelete == null)
                throw new BloomiaNotFoundException("Mood entry not found or does not belong to current user.");

            context.Moods.Remove(moodEntryToDelete);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
