namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTestResult
{
    public class UpdateSelfTestResultNoteCommandHandler(IAppDbContext context)
        : IRequestHandler<UpdateSelfTestResultNoteCommand, UpdateSelfTestResultNoteCommandDto>
    {
        public async Task<UpdateSelfTestResultNoteCommandDto> Handle(UpdateSelfTestResultNoteCommand request, CancellationToken cancellationToken)
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }

            var result = await context.SelfTestResults
                .FirstOrDefaultAsync(x => x.Id == request.ResultId && x.ClientId == client.Id && !x.IsDeleted, cancellationToken);

            if (result == null)
            {
                throw new BloomiaNotFoundException("Self test result not found");
            }

            result.ClientNote = request.ClientNote.Trim();
            result.ModifiedAtUtc = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateSelfTestResultNoteCommandDto
            {
                ResultId = result.Id,
                ClientNote = result.ClientNote
            };
        }
    }
}
