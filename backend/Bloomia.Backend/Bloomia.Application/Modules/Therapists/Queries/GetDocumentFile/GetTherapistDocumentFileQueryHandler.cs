namespace Bloomia.Application.Modules.Therapists.Queries.GetDocumentFile
{
    public sealed class GetTherapistDocumentFileQueryHandler(IAppDbContext context, IAppCurrentUser currentUser, IFileStorageService fileStorageService)
        : IRequestHandler<GetTherapistDocumentFileQuery, TherapistDocumentFileDto>
    {
        public async Task<TherapistDocumentFileDto> Handle(GetTherapistDocumentFileQuery request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new BloomiaBusinessRuleException("AUTH", "User is not authenticated.");

            var document = await context.Documents
                .AsNoTracking()
                .Include(x => x.Therapist)
                .FirstOrDefaultAsync(x => x.Id == request.DocumentId, ct);

            if (document == null)
                throw new BloomiaNotFoundException("Document not found.");

            var isOwner = currentUser.IsTherapist && document.Therapist.UserId == currentUser.UserId;

            if (!currentUser.IsAdmin && !isOwner && !document.IsVisibleToClients)
                throw new BloomiaNotFoundException("Document not found.");

            byte[] content;
            try
            {
                content = await fileStorageService.ReadTherapistDocumentAsync(document.FilePath, ct);
            }
            catch (FileNotFoundException)
            {
                throw new BloomiaNotFoundException("Document file not found.");
            }

            return new TherapistDocumentFileDto
            {
                Content = content,
                FileName = document.FileName
            };
        }
    }
}
