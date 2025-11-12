using Bloomia.Domain.Entities.SelfTestsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.CreateSelfTest
{
    public class CreateSelfTestCommandHandler(IAppDbContext context) : IRequestHandler<CreateSelfTestCommand, CreateSelfTestCommandDto>
    {
        public async Task<CreateSelfTestCommandDto> Handle(CreateSelfTestCommand request, CancellationToken cancellationToken)
        {
            var title = request.Title?.Trim();
           
            var st = await context.SelfTests.Include(x => x.TestQuestions)
                .FirstOrDefaultAsync(x => x.TestName.ToLower() == title.ToLower(), cancellationToken);

            if (st != null) { 
                throw new BloomiaConflictException("Self test with that name already exists.");
            }

            var selfTest = new SelfTestEntity
            {
                TestName = title
            };
            context.SelfTests.Add(selfTest);
            await context.SaveChangesAsync(cancellationToken);
            
            foreach(var s in request.Statements)
            {
                var question = new SelfTestQuestionEntity
                {
                    Text = s.StatementText,
                    SelfTest = selfTest,
                    SelfTestId = selfTest.Id
                };
                context.SelfTestQuestions.Add(question);             
            }
            await context.SaveChangesAsync(cancellationToken);

            var selfTestDto = new CreateSelfTestCommandDto
            {
                Id = selfTest.Id,
                Title = selfTest.TestName

            };
            var selfTestQuestions=await context.SelfTestQuestions.Where(x=>x.SelfTestId==selfTest.Id).ToListAsync(cancellationToken);

            foreach (var item in selfTestQuestions)
            {
                var questionDto = new CreateSelfTestStatementCommandDto
                {
                    SelfTestId = item.SelfTestId,
                    StatementText = item.Text
                };
                selfTestDto.Statements.Add(questionDto);
            }

            return selfTestDto;
        }
    }
}
