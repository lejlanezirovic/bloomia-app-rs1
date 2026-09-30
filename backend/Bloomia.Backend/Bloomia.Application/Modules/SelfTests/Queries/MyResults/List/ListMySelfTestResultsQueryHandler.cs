using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.MyResults.List
{
    public class ListMySelfTestResultsQueryHandler(IAppDbContext context) : IRequestHandler<ListMySelfTestResultsQuery, PageResult<ListMySelfTestResultDto>>
    {
        public async Task<PageResult<ListMySelfTestResultDto>> Handle(ListMySelfTestResultsQuery request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }

            var query = context.SelfTestResults
                .Where(x => x.ClientId == client.Id && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.TestName))
                query = query.Where(x => x.TestAnswers.Any(a => a.SelfTestQuestion.SelfTest.TestName.ToLower().Contains(request.TestName.ToLower())));

            if (request.CompletedFrom.HasValue)
                query = query.Where(x => x.CompletedAt >= request.CompletedFrom.Value);

            if (request.CompletedTo.HasValue)
                query = query.Where(x => x.CompletedAt <= request.CompletedTo.Value);

            if (request.MinAverage.HasValue)
                query = query.Where(x => x.AverageScore >= request.MinAverage.Value);

            if (request.MaxAverage.HasValue)
                query = query.Where(x => x.AverageScore <= request.MaxAverage.Value);

            var projected = query.Select(x => new ListMySelfTestResultDto
            {
                ResultId = x.Id,
                SelfTestId = x.TestAnswers.Select(a => a.SelfTestQuestion.SelfTestId).FirstOrDefault(),
                SelfTestName = x.TestAnswers.Select(a => a.SelfTestQuestion.SelfTest.TestName).FirstOrDefault(),
                CompletedAt = x.CompletedAt,
                AverageScore = x.AverageScore,
                Description = x.Description,
                ClientNote = x.ClientNote
            }).AsNoTracking();

            projected = request.SortByDateDesc
                ? projected.OrderByDescending(x => x.CompletedAt)
                : projected.OrderBy(x => x.CompletedAt);

            return await PageResult<ListMySelfTestResultDto>.FromQueryableAsync(projected, request.Paging, cancellationToken);

        }
    }
}
