using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class TherapistReportConfiguration : IEntityTypeConfiguration<TherapistReportEntity>
    {
        public void Configure(EntityTypeBuilder<TherapistReportEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Therapist)
                .WithMany()
                .HasForeignKey(x => x.TherapistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.FilePath)
            .HasMaxLength(500)
            .IsRequired();

            builder.Property(x => x.FileName)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.TherapistId,
                x.Year,
                x.Month
            })
            .IsUnique();
        }
    }
}
