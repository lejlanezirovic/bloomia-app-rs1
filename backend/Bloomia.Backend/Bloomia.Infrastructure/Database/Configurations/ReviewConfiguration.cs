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

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Client)
                    .WithMany()
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Therapist)
                .WithMany()
                .HasForeignKey(x => x.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
