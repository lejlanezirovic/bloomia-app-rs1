using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTest
{
    public class UpdateSelfTestCommandHandler(IAppDbContext context) : IRequestHandler<UpdateSelfTestCommand, UpdateSelfTestCommandDto>
    {
        public async Task<UpdateSelfTestCommandDto> Handle(UpdateSelfTestCommand request, CancellationToken cancellationToken)
        {
            var selfTest=await context.SelfTests.FirstOrDefaultAsync(x => x.Id == request.SelfTestId, cancellationToken);
            if (selfTest == null)
            {
                throw new BloomiaNotFoundException("Self test doesn't exist in database!");
            }
            var title = request.Title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BloomiaConflictException("Self test title can't be empty!");
            }

            if (selfTest.TestName.ToLower() == request.Title.ToLower())
            {
                throw new BloomiaConflictException("New self test title can't be the same as the old one!");
            }

            selfTest.TestName = title;
            await context.SaveChangesAsync(cancellationToken);

            var selfTestDto = new UpdateSelfTestCommandDto
            {
                SelfTestId = selfTest.Id,
                Title = selfTest.TestName
            };
            return selfTestDto;
        }
    }
}
