using Bloomia.Domain.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
    {
       
        public void Configure(EntityTypeBuilder<AdminEntity> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(x => x.Id);

            builder.HasMany(x=>x.Articles)
                .WithOne(x=>x.Admin)
                .HasForeignKey(x=>x.AdminId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
