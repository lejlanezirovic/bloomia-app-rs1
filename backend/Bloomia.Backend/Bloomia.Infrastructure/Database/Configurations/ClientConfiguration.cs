using Bloomia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(x => x.Id);

            //1:N 
            builder.HasMany(x=>x.Journals)
                .WithOne(x=>x.Client)
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Moods)
                .WithOne(x=>x.Client)
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.SelfTestResults)
                .WithOne(x=>x.Client)
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

           
        }
    }
}
