using Bloomia.Domain.Entities.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentEntity> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.FileExtension)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.DocumentType)
                .IsRequired();

            builder.Property(x => x.UploadedAt)
                .IsRequired();

            builder.HasOne(x => x.Therapist)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.TherapistId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
