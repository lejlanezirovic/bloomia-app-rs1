using Bloomia.Domain.Entities.Payments;
using Bloomia.Domain.Entities.ReviewsFolder;
using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<AppointmentEntity>
    {
        public void Configure(EntityTypeBuilder<AppointmentEntity> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(x=>x.Id);

            builder.HasOne(x => x.Client)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.ChatSessions)
              .WithOne(x => x.Appointment)
              .HasForeignKey(x => x.AppointmentId)
              .OnDelete(DeleteBehavior.NoAction);

            // A slot can hold only one active appointment. Cancelled (Status = 1) and deleted rows
            // are excluded from the index so a released time slot can be booked again.
            builder.HasIndex(x => x.TherapistAvailabilityId)
                    .IsUnique()
                    .HasFilter("[IsDeleted]=0 AND [Status]<>1");


            builder.HasOne(x => x.TherapistAvailability)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.TherapistAvailabilityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
