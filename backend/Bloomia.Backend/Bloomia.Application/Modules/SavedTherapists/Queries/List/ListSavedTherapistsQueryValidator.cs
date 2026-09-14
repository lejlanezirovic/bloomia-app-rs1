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
                .MaximumLength(100).WithMessage("Ime i prezime ne smije biti duze od 100 karaktera")
                .When(x => x.FullName != null);

            RuleFor(x => x.Specialization)
                .MaximumLength(100).WithMessage("Specijalizacija ne smije biti duza od 100 karaktera")
                .When(x => x.Specialization != null);

            RuleFor(x => x.TherapyType)
                .MaximumLength(100).WithMessage("Tip terapije ne smije biti duzi od 100 karaktera")
                .When(x => x.TherapyType != null);

            RuleFor(x => x.MinRating)
                .InclusiveBetween(1, 5).WithMessage("Minimalna ocjena mora biti izmedju 1 i 5")
                .When(x => x.MinRating.HasValue);
        }
    }
}
