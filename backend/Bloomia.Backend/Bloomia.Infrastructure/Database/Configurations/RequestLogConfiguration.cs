using Bloomia.Domain.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLogEntity>
    {
        public void Configure(EntityTypeBuilder<RequestLogEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Method)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Path)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasIndex(x => x.CreatedAtUtc);
        }
    }
}
