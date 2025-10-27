using Bloomia.Domain.Entities.ReviewsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.ToTable("Reviews");

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.HasOne(r => r.Appointment)
                .WithOne(a => a.Review)
                .HasForeignKey<ReviewEntity>(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
