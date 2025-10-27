using Bloomia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
         
            builder.ToTable("Users");//preimenovati

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Firstname)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Lastname)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(x=> x.Username)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(x => x.PasswordHash)
                    .IsRequired();

            builder.Property(x => x.IsEnabled).HasDefaultValue(true);

            builder.HasOne<TherapistEntity>()
                  .WithOne(t => t.User)
                  .HasForeignKey<TherapistEntity>(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ClientEntity>()
                  .WithOne(c => c.User)
                  .HasForeignKey<ClientEntity>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            //1:N
            builder.HasMany(x=>x.RefreshTokens)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
