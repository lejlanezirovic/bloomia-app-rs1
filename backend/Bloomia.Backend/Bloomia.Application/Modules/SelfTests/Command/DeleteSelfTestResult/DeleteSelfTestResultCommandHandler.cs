using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.DeleteSelfTestResult
{
    public class DeleteSelfTestResultCommandHandler(IAppDbContext context) : IRequestHandler<DeleteSelfTestResultCommand, string>
    {
        public async Task<string> Handle(DeleteSelfTestResultCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }

            var result = await context.SelfTestResults
                .FirstOrDefaultAsync(x => x.Id == request.ResultId && x.ClientId == client.Id && !x.IsDeleted, cancellationToken);

            if (result == null)
            {
                throw new BloomiaNotFoundException("Self test result not found");
            }

            result.IsDeleted = true;
            result.ModifiedAtUtc = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return $"Self test result with id {result.Id} removed successfully";
        }
    }
}
