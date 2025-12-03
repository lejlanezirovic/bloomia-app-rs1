using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.List
{
    public class ListSavedTherapistsQueryDto
    {
       public List<ListSavedTherapistInfoDto> SavedTherapists { get; set; } = new List<ListSavedTherapistInfoDto>();
    }
    public class ListSavedTherapistInfoDto
    {
        public int TherapistId { get; set; }
        public string Fullname { get; set; }
        public string Specialization { get; set; }
        public string Description { get; set; }
        public double RatingAvg { get; set; }
        public List<ListTherapistTherapyTypesQueryDto> MYTherapyTypes { get; set; } = new List<ListTherapistTherapyTypesQueryDto>();
    }

    public class ListTherapistTherapyTypesQueryDto
    {
        public int TherapistId { get; set; }
        public string TherapyTypeName { get; set; }

    }
}
