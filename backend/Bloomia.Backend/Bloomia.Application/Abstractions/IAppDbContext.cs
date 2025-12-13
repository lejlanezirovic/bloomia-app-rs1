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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Bloomia.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    public DbSet<ClientEntity> Clients { get; }
    public DbSet<TherapistEntity> Therapists { get; }
    public DbSet<DocumentEntity> Documents { get; }
    public DbSet<GenderEntity> Genders { get; }
    public DbSet<LanguageEntity> Languages { get; }
    public DbSet<LocationEntity> Locations { get; }
    public DbSet<JournalEntity> Journals { get; }
    public DbSet<JournalAnswerEntity> JournalAnswers { get; }
    public DbSet<JournalQuestionEntity> JournalQuestions { get; }
    public DbSet<MoodEntity> Moods { get; }
    public DbSet<ReviewEntity> Reviews { get; }
    public DbSet<SelfTestEntity> SelfTests { get; }
    public DbSet<SelfTestQuestionEntity> SelfTestQuestions { get; }
    public DbSet<SelfTestAnswerEntity> SelfTestAnswers { get; }
    public DbSet<SelfTestResultEntity> SelfTestResults { get; }
    public DbSet<AppointmentEntity> Appointments { get; }
    public DbSet<ChatSessionEntity> ChatSessions { get; }
    public DbSet<MessageEntity> Messages { get; }
    public DbSet<TherapistAvailabilityEntity> TherapistAvailabilities { get; }
    public DbSet<TherapyTypeEntity> TherapyTypes { get; }
    public DbSet<TherapistsTherapyTypesEntity> TherapistsTherapyTypes { get; }
    public DbSet<SavedTherapistsEntity> SavedTherapists { get; }
    public DbSet<PaymentEntity> Payments  { get; }
    public DbSet<AdminEntity> Admins { get; }
    public DbSet<RoleEntity> Roles { get; }
    public DbSet<ArticleEntity> Articles { get; }
    public DbSet<DirectChatEntity> DirectChats { get; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}