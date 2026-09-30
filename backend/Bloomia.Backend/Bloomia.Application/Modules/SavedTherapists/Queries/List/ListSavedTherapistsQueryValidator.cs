using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SavedTherapists.Queries.List
{
    public class ListSavedTherapistsQueryValidator : AbstractValidator<ListSavedTherapistsQuery>
    {
        public ListSavedTherapistsQueryValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("First and last name must not exceed 100 characters")
                .When(x => x.FullName != null);

            RuleFor(x => x.Specialization)
                .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters")
                .When(x => x.Specialization != null);

            RuleFor(x => x.TherapyType)
                .MaximumLength(100).WithMessage("Therapy type must not exceed 100 characters")
                .When(x => x.TherapyType != null);

            RuleFor(x => x.MinRating)
                .InclusiveBetween(1, 5).WithMessage("Minimum rating must be between 1 and 5")
                .When(x => x.MinRating.HasValue);
        }
    }
}
