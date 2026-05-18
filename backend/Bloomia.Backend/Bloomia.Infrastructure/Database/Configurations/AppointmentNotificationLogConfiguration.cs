using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Sessions;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class AppointmentNotificationLogConfiguration : IEntityTypeConfiguration<AppointmentNotificationLogEntity>
    {
        public void Configure(EntityTypeBuilder<AppointmentNotificationLogEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RecipientEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(2000);

            builder.HasOne(x => x.Appointment)
                .WithMany(x => x.NotificationLogs)
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
