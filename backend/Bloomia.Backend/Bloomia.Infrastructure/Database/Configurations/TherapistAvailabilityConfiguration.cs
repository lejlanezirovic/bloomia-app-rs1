using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class TherapistAvailabilityConfiguration : IEntityTypeConfiguration<TherapistAvailabilityEntity>
    {
        public void Configure(EntityTypeBuilder<TherapistAvailabilityEntity> builder)
        {
            builder.ToTable("TherapistAvailabilities");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Therapist)
                  .WithMany(x => x.Availability)
                 .HasForeignKey(x => x.TherapistId)
                 .OnDelete(DeleteBehavior.Restrict);

           builder.HasOne(x=>x.Appointment)
                .WithOne(x=>x.TherapistAvailability)
                .HasForeignKey<AppointmentEntity>(x=>x.TherapistAvailabilityId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
