using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Command.Add
{
    public class AddTherapistToSavedTherapistsCommandDto
    {
        public int TherapistId { get; set; }
        public string Fullname { get; set; }
        public string Specialization { get; set; }
        public string Description { get; set; }
        public double RatingAvg { get; set; }
        public List<ListTherapistTherapyTypesQueryDto> MYTherapyTypes { get; set; } = new List<ListTherapistTherapyTypesQueryDto>();


    }
}
