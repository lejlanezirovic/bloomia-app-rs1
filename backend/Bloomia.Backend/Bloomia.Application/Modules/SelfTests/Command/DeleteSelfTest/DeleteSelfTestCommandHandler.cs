using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.DeleteSelfTest
{
    public class DeleteSelfTestCommandHandler(IAppDbContext context) : IRequestHandler<DeleteSelfTestCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteSelfTestCommand request, CancellationToken cancellationToken)
        {
           var selfTest= await context.SelfTests.FirstOrDefaultAsync(x => x.Id == request.SelfTestId, cancellationToken);
            if (selfTest==null)
            {
                throw new BloomiaNotFoundException("Self test doesn't exist in database!");
            }

            var questions = await context.SelfTestQuestions
                .Where(x => x.SelfTestId == selfTest.Id).ToListAsync(cancellationToken);
            foreach (var q in questions)
            {
                q.IsDeleted = true;
            }
            selfTest.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
