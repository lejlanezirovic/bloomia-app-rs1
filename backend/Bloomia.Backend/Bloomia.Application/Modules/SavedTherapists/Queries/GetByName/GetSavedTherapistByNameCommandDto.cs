using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.GetByName
{
    public class GetSavedTherapistByNameCommandDto
    {
        public int TherapistId { get; set; }
        public string TherapistProfilePictureUrl { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Description { get; set; }
        public double RatingAvg { get; set; }
        public List<ListTherapistTherapyTypesQueryDto> MyTherapyTypes { get; set; } = new List<ListTherapistTherapyTypesQueryDto>();
    }
}
