using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Commands.Create
{
    public sealed class CreateMoodEntryCommandValidator : AbstractValidator<CreateMoodEntryCommand>
    {
        public CreateMoodEntryCommandValidator()
        {
            RuleFor(x => x.Happiness).InclusiveBetween(1, 5);
            RuleFor(x => x.Sadness).InclusiveBetween(1, 5);
            RuleFor(x => x.Anger).InclusiveBetween(1, 5);
            RuleFor(x => x.Stress).InclusiveBetween(1, 5);
            RuleFor(x => x.Depression).InclusiveBetween(1, 5);
            RuleFor(x => x.Anxiety).InclusiveBetween(1, 5);
        }
    }
}
