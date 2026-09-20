using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.Update.UpdateTherapistVerification
{
    public class UpdateTherapisVerificationCommand : IRequest
    {
        public int TherapistId { get; set; }
        public bool IsVerified { get; set; }
    }
}
