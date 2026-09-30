using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.RemoveAll
{
    public class RemoveAllSavedTherapistsCommandHandler(IAppDbContext context) : IRequestHandler<RemoveAllSavedTherapistsCommand, string>
    {
        public async Task<string> Handle(RemoveAllSavedTherapistsCommand request, CancellationToken cancellationToken)
        {
            var client=await context.Clients.Include(x=> x.User).Where(x=>x.User.Id==request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }

            var clientSavedTherapists=await context.SavedTherapists.Include(x=>x.Client).Include(x=>x.Therapist)
                            .Where(x=>x.Client.Id==client.Id && !x.IsDeleted).ToListAsync(cancellationToken);

            if (clientSavedTherapists.Count == 0)
            {
                throw new BloomiaNotFoundException("No saved therapists for this client");
            }
                        
            foreach(var svd in clientSavedTherapists)
            {
                svd.IsDeleted = true;
                svd.ModifiedAtUtc = DateTime.UtcNow;
            }
            await context.SaveChangesAsync(cancellationToken);
            return "All saved therapists for the client were successfully removed";
        }
    }
}
