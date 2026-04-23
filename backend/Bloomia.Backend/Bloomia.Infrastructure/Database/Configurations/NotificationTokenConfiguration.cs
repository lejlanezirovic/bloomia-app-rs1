using Bloomia.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class NotificationTokenConfiguration : IEntityTypeConfiguration<NotificationTokenEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationTokenEntity> builder)
        {
            builder.ToTable("NotificationTokens");

            builder.Property(x => x.Token).IsRequired();

            builder.Property(x=>x.IsActive).IsRequired();
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
