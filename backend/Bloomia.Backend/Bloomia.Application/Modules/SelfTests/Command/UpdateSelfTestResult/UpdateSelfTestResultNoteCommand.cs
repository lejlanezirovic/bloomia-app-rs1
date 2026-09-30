namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTestResult
{
    public class UpdateSelfTestResultNoteCommand : IRequest<UpdateSelfTestResultNoteCommandDto>
    {
        [JsonIgnore]
        public int ResultId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public string ClientNote { get; set; }
    }

    public class UpdateSelfTestResultNoteCommandDto
    {
        public int ResultId { get; set; }
        public string ClientNote { get; set; }
    }
}
