using Bloomia.Domain.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                 .HasPrecision(18, 2)
                 .IsRequired();

            builder.HasOne(x => x.Appointment)
           .WithOne()
           .HasForeignKey<PaymentEntity>(x => x.AppointmentId)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
