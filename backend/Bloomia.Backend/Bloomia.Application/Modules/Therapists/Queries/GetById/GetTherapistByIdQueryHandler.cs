using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.Users.Queries.GetById;

namespace Bloomia.Application.Modules.Therapists.Queries.GetById
{
    public sealed class GetTherapistByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetTherapistByIdQuery, GetTherapistByIdQueryDto>
    {
        public async Task<GetTherapistByIdQueryDto> Handle(GetTherapistByIdQuery request, CancellationToken ct)
        {
            var therapist = await context.Therapists
                .Include(x => x.User)
                .Include(x => x.MyTherapyTypesList)
                    .ThenInclude(tt => tt.TherapyType)
                .Include(x => x.Availability)
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found.");

            var therapistDto = new GetTherapistByIdQueryDto
            {
                Id = request.Id,
                Firstname = therapist.User.Firstname,
                Lastname = therapist.User.Lastname,
                Username = therapist.User.Username,
                Fullname = therapist.User.Fullname,
                Email = therapist.User.Email,
                PhoneNumber = therapist.User.PhoneNumber,
                ProfileImage = therapist.User.ProfileImage,
                Specialization = therapist.Specialization,
                Description = therapist.Description,
                RatingAvg = therapist.RatingAvg,
                IsVerified = therapist.isVerified,
                Documents = therapist.Documents
                            .Select(td => new TherapistDocumentDto
                            {
                                Id = td.Id,
                                DocumentType = td.DocumentType.ToString(),
                                FileName = td.FileName,
                                FilePath = td.FilePath,
                            }).ToList(),
                TherapyTypes = therapist.MyTherapyTypesList
                            .Select(tt => new TherapyTypeDto
                            {
                                Id = tt.TherapyTypeId,
                                Name = tt.TherapyType.TherapyName
                            }).ToList(),
                Availability = therapist.Availability
                            .Select(a => new TherapistAvailabilityDto
                            {
                                Id = a.Id,
                                Date = a.Date,
                                StartTime = a.StartTime,
                                IsBooked = a.IsBooked,
                            }).ToList()
            };

            return therapistDto;
        }
    }
}
