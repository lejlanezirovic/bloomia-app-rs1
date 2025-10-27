using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Payments
{
    public class PaymentEntity
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public AppointmentEntity Appointment { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }// e.g., Credit Card, PayPal, Stripe, etc.
        public string PaymentStatus { get; set; } // e.g., Pending, Completed, Failed
        
        public DateTime PaymentDate { get; set; }=DateTime.UtcNow;

    }
}
