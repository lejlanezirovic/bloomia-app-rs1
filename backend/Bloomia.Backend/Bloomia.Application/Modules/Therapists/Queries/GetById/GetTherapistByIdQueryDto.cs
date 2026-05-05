using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Queries.GetById
{
    public class GetTherapistByIdQueryDto
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public string? Specialization { get; set; } 
        public string? Description { get; set; }
        public float RatingAvg { get; set; }
        public bool IsVerified { get; set; }
        public List<TherapistDocumentDto> Documents { get; set; } = new();
        public List<TherapyTypeDto> TherapyTypes { get; set; } = new List<TherapyTypeDto>();
        public List<TherapistAvailabilityDto> Availability { get; set; } = new List<TherapistAvailabilityDto>();
    }

    public class TherapistDocumentDto
    {
        public int Id { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    public class TherapyTypeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class TherapistAvailabilityDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public bool IsBooked { get; set; }

    }
}
