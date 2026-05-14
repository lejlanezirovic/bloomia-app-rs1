using Bloomia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class TherapistConfiguration : IEntityTypeConfiguration<TherapistEntity>
    {
        public void Configure(EntityTypeBuilder<TherapistEntity> builder)
        {
            builder.ToTable("Therapists");

            builder.HasKey(t => t.Id);

            builder.HasMany(x => x.Documents)
                .WithOne(x => x.Therapist)
                .HasForeignKey(x => x.TherapistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x=>x.Availability)
                .WithOne(x=>x.Therapist)
                .HasForeignKey(x=>x.TherapistId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
