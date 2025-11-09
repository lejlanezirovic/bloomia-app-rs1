using Bloomia.Domain.Entities.Admin;
using Bloomia.Domain.Entities.Basics;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.JournalsFolder;
using Bloomia.Domain.Entities.SelfTestsFolder;
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

            await SeedSelfTestsAndSelfTestQuestions(context);
            await SeedJournalQuestions(context);
            await SeedLanguage(context);
            await SeedGenders(context);
            await SeedLocation(context);
            await SeedTherapyTypes(context);
            await SeedDocuments(context);
            await SeedUsersAsync(context);
        }

        public static async Task SeedDocuments(DatabaseContext context)
        {
            if (await context.Documents.AnyAsync())
                return;

            var demoDocument1 = new DocumentEntity
            {
                DocumentType = "CV",
                FilePath="/uploads/documents/CV.pdf",
                FileName = "CV.pdf",
                FileExtension = ".pdf",
                UploadedAt=DateTime.UtcNow
            };

            var demoDocument2 = new DocumentEntity 
            {
                DocumentType = "CV",
                FilePath = "/uploads/documents/CV2.pdf",
                FileName = "CV2.pdf",
                FileExtension = ".pdf",
                UploadedAt = DateTime.UtcNow 
            };

            context.Documents.AddRange(demoDocument1, demoDocument2);
            await context.SaveChangesAsync();

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
            context.Genders.AddRange(male, female, other);
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
            context.Languages.AddRange(lang1, lang2);
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
        public static async Task SeedJournalQuestions(DatabaseContext context)
        {
            if(await context.JournalQuestions.AnyAsync())
                return;

            var q1 = new JournalQuestionEntity
            {
                QuestionText = "What is going well in my life right now?"
            };
            var q2 = new JournalQuestionEntity
            {
                QuestionText = "What can I do to be 1% closer to my goal(s)?"
            };
            var q3 = new JournalQuestionEntity
            {
                QuestionText = "What worries me the most currently? What are my steps to overcome this?"
            };
            var q4 = new JournalQuestionEntity
            {
                QuestionText = "What personal achievements am I most proud of in the past period?"
            };
            var q5 = new JournalQuestionEntity
            {
                QuestionText = "What am I grateful for today?"
            };
            context.JournalQuestions.AddRange(q1,q2,q3,q4,q5);
            await context.SaveChangesAsync();
            
        }
        public static async Task SeedSelfTestsAndSelfTestQuestions(DatabaseContext context)
        {
            if (await context.SelfTests.AnyAsync())
                return;
            if (await context.SelfTestQuestions.AnyAsync())
                return;

            var selfTest1 = new SelfTestEntity
            {
                TestName = "Emotional Regulation Test"
            };
            var selfTest2 = new SelfTestEntity
            {
                TestName = "Relationship and Communication Test"
            };
            var selfTest3 = new SelfTestEntity
            {
                TestName = "Burnout Test"
            };
            context.SelfTests.AddRange(selfTest1,selfTest2,selfTest3);


            var q1 = new SelfTestQuestionEntity
            {
                Text = "My mood changes quickly.",
                SelfTest = selfTest1,
                SelfTestId = selfTest1.Id

            };
            var q2 = new SelfTestQuestionEntity
            {
                Text = "My emotions frequently overwhelm me.",
                SelfTest = selfTest1,
                SelfTestId = selfTest1.Id
            };
            var q3 = new SelfTestQuestionEntity
            {
                Text = "I find it hard to express what I feel.",
                SelfTest = selfTest1,
                SelfTestId = selfTest1.Id
            };
            var q4 = new SelfTestQuestionEntity
            {
                Text = "I don't often experience a sense of inner peace.",
                SelfTest = selfTest1,
                SelfTestId = selfTest1.Id
            };
            var q5 = new SelfTestQuestionEntity
            {
                Text = "I can't identify my emotions clearly when they arise.",
                SelfTest = selfTest1,
                SelfTestId = selfTest1.Id
            };


            var q6 = new SelfTestQuestionEntity
            {
                Text = "I find it difficult to tell others how I truly feel.",
                SelfTest = selfTest2,
                SelfTestId = selfTest2.Id
            };
            var q7 = new SelfTestQuestionEntity
            {
                Text = "I’m afraid of conflicts and tend to avoid them.",
                SelfTest = selfTest2,
                SelfTestId = selfTest2.Id
            };
            var q8 = new SelfTestQuestionEntity
            {
                Text = "I struggle to set boundaries in relationships.",
                SelfTest = selfTest2,
                SelfTestId = selfTest2.Id
            };
            var q9 = new SelfTestQuestionEntity
            {
                Text = "I don't express my needs clearly in relationships.",
                SelfTest = selfTest2,
                SelfTestId = selfTest2.Id
            };
            var q10 = new SelfTestQuestionEntity
            {
                Text = "I don't communicate openly.",
                SelfTest = selfTest2,
                SelfTestId = selfTest2.Id
            };


            var q11 = new SelfTestQuestionEntity
            {
                Text = "I feel tired even after getting enough sleep.",
                SelfTest = selfTest3,
                SelfTestId = selfTest3.Id
            };
            var q12 = new SelfTestQuestionEntity
            {
                Text = "I have no energy for small daily tasks.",
                SelfTest = selfTest3,
                SelfTestId = selfTest3.Id
            };
            var q13 = new SelfTestQuestionEntity
            {
                Text = "I find it difficult to concentrate on simple things.",
                SelfTest = selfTest3,
                SelfTestId = selfTest3.Id
            };
            var q14 = new SelfTestQuestionEntity
            {
                Text = "My work or daily responsibilities drain me.",
                SelfTest = selfTest3,
                SelfTestId = selfTest3.Id
            };
            var q15 = new SelfTestQuestionEntity
            {
                Text = "It’s hard for me to find motivation to start the day.",
                SelfTest = selfTest3,
                SelfTestId = selfTest3.Id
            };
            context.SelfTestQuestions.AddRange(q1,q2,q3,q4,q5,q6,q7,q8,q9,q10,q11,q12,q13,q14,q15);
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
                    RoleName = RoleName.ADMIN.ToString()
                };
                context.Roles.Add(role);
            }
            if (clientRole == null)
            {
                var role = new RoleEntity
                {
                    RoleName = RoleName.CLIENT.ToString()
                };
                context.Roles.Add(role);
            }
            if (therapistRole == null)
            {
                var role = new RoleEntity
                {
                    RoleName = RoleName.THERAPIST.ToString()
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
                Fullname="Lejla Admin",
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
                Fullname = "Test Test",
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
                Fullname = "T U",
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
