using Bloomia.Domain.Entities.Basics;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Seeders
{
    public static class DynamicDataSeedersForBloomia
    {
        public static async Task SeedAsync(DatabaseContext context)
        {
            // Osiguraj da baza postoji (bez migracija)
            await context.Database.EnsureCreatedAsync();

            await SeedLanguage(context);
            await SeedGenders(context);
            await SeedLocation(context);
            await SeedTherapyTypes(context);
            await SeedUsersAsync(context);
        }
        public static async Task SeedGenders(DatabaseContext context)
        {
            if (await context.Genders.AnyAsync())
                return;

            var male = new GenderEntity { 
                Name="MALE"
            };
            var female = new GenderEntity
            {
                Name = "FEMALE"
            };
            var other = new GenderEntity
            {
                Name = "OTHER"
            };
            context.Genders.AddRange(male, female,other);
            await context.SaveChangesAsync();

        }
        public static async Task SeedLanguage(DatabaseContext context)
        {
            if (await context.Languages.AnyAsync())
                return;

            var lang1 = new LanguageEntity
            {
                Name = "Bosnian"
            };
            var lang2 = new LanguageEntity
            {
                Name = "English"
            };
            context.Languages.AddRange(lang1,lang2);
            await context.SaveChangesAsync();
        }
        public static async Task SeedLocation(DatabaseContext context)
        {
            if (await context.Locations.AnyAsync())
                return;

            var loc1= new LocationEntity
            {
                City = "Sarajevo",
                Country = "Bosnia and Herzegovina"
            };
            context.Locations.AddRange(loc1);
            await context.SaveChangesAsync();
        }
        public static async Task SeedTherapyTypes(DatabaseContext context)
        {
            if (await context.TherapyTypes.AnyAsync())
                return;

            var therapyType1 = new TherapyTypeEntity
            {
                TherapyName = TherapyType.BRAIN_STIMULATION_THERAPY.ToString(),
                Description = "Therapy that uses electrical or magnetic stimulation to target specific areas of the brain."
            };
            var therapyType2 = new TherapyTypeEntity
            {
                TherapyName = TherapyType.COGNITIVE_BEHAVIORAL_THERAPY.ToString()
            };
            var therapyType3 = new TherapyTypeEntity
            {
                TherapyName = TherapyType.PSYCHODYNAMIC_THERAPY.ToString()
            };
            var therapyType4 = new TherapyTypeEntity
            {
                TherapyName = TherapyType.GROUP_THERAPY.ToString()
            };
            context.TherapyTypes.AddRange(therapyType1, therapyType2, therapyType3, therapyType4);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUsersAsync(DatabaseContext context)
        {
            if(await context.Users.AnyAsync())
                return;

            //ako nema usera dodaj ih
            //pronadji uloge u bazi 
            //ako ne postoje dodaj ih
            //zatim ih ponovo ucitavamo 
            var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.RoleName == "ADMIN");
            var clientRole=await context.Roles.FirstOrDefaultAsync(x=>x.RoleName=="CLIENT");
            var therapistRole=await context.Roles.FirstOrDefaultAsync(x=>x.RoleName=="THERAPIST");


            if (adminRole == null)
            {
                var role = new RoleEntity
                {
                    RoleName = "ADMIN"
                };
                context.Roles.Add(role);
            }
            if (clientRole == null)
            {
                var role = new RoleEntity
                {
                    RoleName = "CLIENT"
                };
                context.Roles.Add(role);
            }
            if (therapistRole == null)
            {
                var role = new RoleEntity
                {
                    RoleName = "THERAPIST"
                };
                context.Roles.Add(role);
            }
            await context.SaveChangesAsync();


            adminRole=await context.Roles.FirstAsync(x=>x.RoleName=="ADMIN");
            clientRole=await context.Roles.FirstAsync(x=>x.RoleName=="CLIENT");
            therapistRole=await context.Roles.FirstAsync(x=>x.RoleName=="THERAPIST");

            var hasher=new PasswordHasher<UserEntity>();

            var adminUser = new UserEntity
            {
                Firstname = "Lejla",
                Lastname = "Admin",
                Username = "admin",
                Email = "lejla.nezirovic@edu.fit.ba",
                PasswordHash = hasher.HashPassword(null!, "Admin123!"),
                RoleId = adminRole.Id,
                IsEnabled = true,
                GenderId=2,
                LanguageId=1,
                LocationId=1
            };

            var clientUser = new UserEntity
            {
                Firstname = "Test",
                Lastname = "Test",
                Username = "Test",
                Email = "test@gmail.com",
                PasswordHash = hasher.HashPassword(null!, "test123!"),
                RoleId = clientRole.Id,
                IsEnabled = true,
                GenderId = 2,
                LanguageId = 1,
                LocationId = 1
            };

            var therapistUser = new UserEntity
            {
                Firstname = "T",
                Lastname = "U",
                Username = "TU",
                Email = "therapist.user@gmail.com",
                PasswordHash = hasher.HashPassword(null!, "therrapist1!"),
                RoleId = therapistRole.Id,
                IsEnabled = true,
                GenderId = 2,
                LanguageId = 1,
                LocationId = 1
            };

            context.Users.AddRange(adminUser, clientUser, therapistUser);
            await context.SaveChangesAsync();
            Console.WriteLine("Dynamic seed for Bloomia: demo usersE added.");
        }
    }
}
