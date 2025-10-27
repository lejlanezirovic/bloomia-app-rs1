using Bloomia.Application.Abstractions;
using Bloomia.Domain.Entities;
using Bloomia.Domain.Entities.Admin;
using Bloomia.Domain.Entities.Basics;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Entities.JournalsFolder;
using Bloomia.Domain.Entities.MoodsFolder;
using Bloomia.Domain.Entities.Payments;
using Bloomia.Domain.Entities.ReviewsFolder;
using Bloomia.Domain.Entities.SelfTestsFolder;
using Bloomia.Domain.Entities.Sessions;
using Bloomia.Domain.Entities.TherapistRelated;

namespace Bloomia.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
   
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<TherapistEntity> Therapists => Set<TherapistEntity>();
    public DbSet<DocumentEntity> Documents => Set<DocumentEntity>();
    public DbSet<GenderEntity> Genders=> Set<GenderEntity>();
    public DbSet<LanguageEntity> Languages => Set<LanguageEntity>();
    public DbSet<LocationEntity> Locations => Set<LocationEntity>();
    public DbSet<JournalEntity> Journals => Set<JournalEntity>();
    public DbSet<JournalAnswerEntity> JournalAnswers => Set<JournalAnswerEntity>();
    public DbSet<JournalQuestionEntity> JournalQuestions => Set<JournalQuestionEntity>();
    public DbSet<MoodEntity> Moods => Set<MoodEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<SelfTestEntity> SelfTests => Set<SelfTestEntity>();
    public DbSet<SelfTestQuestionEntity> SelfTestQuestions => Set<SelfTestQuestionEntity>();
    public DbSet<SelfTestAnswerEntity> SelfTestAnswers => Set<SelfTestAnswerEntity>();
    public DbSet<SelfTestResultEntity> SelfTestResults => Set<SelfTestResultEntity>();
    public DbSet<AppointmentEntity> Appointments => Set<AppointmentEntity>();
    public DbSet<ChatSessionEntity> ChatSessions => Set<ChatSessionEntity>();
    public DbSet<MessageEntity> Messages=> Set<MessageEntity>();
    public DbSet<TherapistAvailabilityEntity> TherapistAvailabilities => Set<TherapistAvailabilityEntity>();
    public DbSet<TherapyTypeEntity> TherapyTypes => Set<TherapyTypeEntity>();
    public DbSet<TherapistsTherapyTypesEntity> TherapistsTherapyTypes => Set<TherapistsTherapyTypesEntity>();
    public DbSet<SavedTherapistsEntity> SavedTherapists => Set<SavedTherapistsEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<AdminEntity> Admins => Set<AdminEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    public DbSet<ArticleEntity> Articles => Set<ArticleEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}