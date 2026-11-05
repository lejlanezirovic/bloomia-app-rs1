namespace Bloomia.Application.Modules.Therapists.Queries.GetDocumentFile
{
    public sealed class GetTherapistDocumentFileQuery : IRequest<TherapistDocumentFileDto>
    {
        public int DocumentId { get; set; }
    }
}
