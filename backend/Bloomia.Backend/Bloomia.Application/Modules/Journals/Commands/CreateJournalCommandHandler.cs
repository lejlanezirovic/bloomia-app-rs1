using Bloomia.Domain.Entities.JournalsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Journals.Commands
{
    public class CreateJournalCommandHandler(IAppDbContext context) : IRequestHandler<CreateJournalCommand, CreateJournalCommandDto>
    {
        public async Task<CreateJournalCommandDto> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);
            if (client == null)
                throw new ValidationException(message: "Couldn't find the client");

            var journal = new JournalEntity
            {
                ClientId = client.Id,
                Client = client,
                CreatedAt = DateTime.UtcNow,
                Title = request.Title
            };
            context.Journals.Add(journal);
            await context.SaveChangesAsync(cancellationToken);

            //uzmemo id pitanja
            var questionIds = request.ClientsAnswers.Select(x => x.QuestionId).Distinct().ToList();
            var existingQuestions = await context.JournalQuestions.Where(x => questionIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (existingQuestions.Count != questionIds.Count)
                throw new ValidationException(message: "One or more questionIds are invalid");

            foreach(var ans in request.ClientsAnswers)
            {
                var answer = new JournalAnswerEntity
                {
                    Journal = journal,
                    JournalId = journal.Id,
                    AnswerText = ans.AnswerText,
                    JournalQuestionId = ans.QuestionId
                };
                context.JournalAnswers.Add(answer);
            }

            await context.SaveChangesAsync(cancellationToken);
            var journalDto = new CreateJournalCommandDto
            {
                JournalId = journal.Id,
                JournalName = journal.Title,
                CreatedAt = journal.CreatedAt

            };
            var answers = await context.JournalAnswers.Include(x=>x.JournalQuestion).Where(x=>x.JournalId==journal.Id).ToListAsync(cancellationToken);
            foreach(var item in answers)
            {
                var answerDto = new CreateJournalAnswerCommandDto
                {
                    AnswerText = item.AnswerText,
                    QuestionId = item.JournalQuestionId,
                    QuestionText = item.JournalQuestion.QuestionText
                };
                journalDto.JournalAnswers.Add(answerDto);
            }
         return journalDto;

        }
    }
}
