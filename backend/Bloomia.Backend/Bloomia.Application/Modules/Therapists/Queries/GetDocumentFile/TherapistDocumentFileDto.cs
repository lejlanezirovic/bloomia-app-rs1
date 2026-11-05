namespace Bloomia.Application.Modules.Therapists.Queries.GetDocumentFile
{
    public sealed class TherapistDocumentFileDto
    {
        public byte[] Content { get; set; } = [];
        public string FileName { get; set; } = string.Empty;
    }
}
