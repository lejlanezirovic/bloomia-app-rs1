using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class DirectChatConfiguration : IEntityTypeConfiguration<DirectChatEntity>
    {
        public void Configure(EntityTypeBuilder<DirectChatEntity> builder)
        {
            builder.ToTable("DirectChats");
            builder.HasKey(x => x.Id);

             builder.HasOne(x=>x.Client)
                .WithMany()
                .HasForeignKey(x=>x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x=>x.Therapist)
                .WithMany()
                .HasForeignKey(x=>x.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x=>x.Messages)
                .WithOne(x=>x.DirectChat)
                .HasForeignKey(x=>x.DirectChatId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    /*NEMA kolekcije poruka u ClientEntity i TherapistEntity jer:

Message nije direktno vezan za klijenta/terapeuta

Relationship ide preko DirectChat

Ovo je najčišći i najstandardniji dizajn

Izbjegava se nepotrebna kompleksnost i migracije*/
}
