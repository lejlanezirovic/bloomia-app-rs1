namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTestResult
{
    public class UpdateSelfTestResultNoteCommandValidator : AbstractValidator<UpdateSelfTestResultNoteCommand>
    {
        public UpdateSelfTestResultNoteCommandValidator()
        {
            RuleFor(x => x.ResultId).GreaterThan(0).WithMessage("ResultId must be greater than 0");

            RuleFor(x => x.ClientNote)
                .NotEmpty().WithMessage("Note is required")
                .MaximumLength(500).WithMessage("Note must not be longer than 500 characters");
        }
    }
}
