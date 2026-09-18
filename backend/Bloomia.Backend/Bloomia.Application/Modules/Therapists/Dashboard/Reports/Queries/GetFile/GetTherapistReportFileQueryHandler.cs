using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Reports.Queries.GetFile
{
    public class GetTherapistReportFileQueryHandler(IAppDbContext ctx, IAppCurrentUser currentUser, IFileStorageService fileStorageService) : IRequestHandler<GetTherapistReportFileQuery, TherapistReportFileDto>
    {
        public async Task<TherapistReportFileDto> Handle(GetTherapistReportFileQuery request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");
           
            var report = await ctx.TherapistReports
               .AsNoTracking()
               .FirstOrDefaultAsync(
                   x =>
                       x.Id == request.Id &&
                       x.Therapist.UserId ==
                           currentUser.UserId,
                   ct);

            if (report == null)
            {
                throw new BloomiaNotFoundException(
                    "Report not found.");
            }

            var content =
                await fileStorageService.ReadReportAsync(
                    report.FilePath,
                    ct);

            return new TherapistReportFileDto
            {
                Content = content,
                FileName = report.FileName
            };
        }
    }
}
